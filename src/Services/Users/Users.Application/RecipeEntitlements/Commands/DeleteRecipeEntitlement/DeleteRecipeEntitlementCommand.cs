using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.Users.Application.Exceptions;
using CulinaCloud.Users.Application.Interfaces;
using CulinaCloud.Users.Domain.Entities;
using CulinaCloud.Users.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.Users.Application.RecipeEntitlements.Commands.DeleteRecipeEntitlement
{
    public class DeleteRecipeEntitlementCommand : IRequest<DeleteRecipeEntitlementResponse>
    {
        public Guid Id { get; set; }
    }

    public class DeleteRecipeEntitlementCommandHandler : IRequestHandler<DeleteRecipeEntitlementCommand, DeleteRecipeEntitlementResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteRecipeEntitlementCommandHandler(
            IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<DeleteRecipeEntitlementResponse> Handle(DeleteRecipeEntitlementCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.RecipeEntitlements
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(
                    nameof(RecipeEntitlement),
                    request.Id.ToString()
                );
            }

            var recipeAuthor = await _context.RecipeEntitlements
                    .SingleOrDefaultAsync(x => x.RecipeId == entity.RecipeId && x.Type == RecipeEntitlementType.AUTHOR, cancellationToken);

            if (entity.Id == recipeAuthor.Id)
            {
                throw new CanNotDeleteRecipeAuthorException(entity.RecipeId);
            }

            _context.RecipeEntitlements.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            var response = _mapper.Map<DeleteRecipeEntitlementResponse>(entity);
            return response;
        }
    }
}