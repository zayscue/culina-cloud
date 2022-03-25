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

namespace CulinaCloud.Users.Application.RecipeEntitlements.Commands.UpdateRecipeEntitlement
{
    public class UpdateRecipeEntitlementCommand : IRequest<UpdateRecipeEntitlementResponse>
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string GrantedBy { get; set; }
    }

    public class UpdateRecipeEntitlementCommandHandler : IRequestHandler<UpdateRecipeEntitlementCommand, UpdateRecipeEntitlementResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateRecipeEntitlementCommandHandler(
            IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<UpdateRecipeEntitlementResponse> Handle(UpdateRecipeEntitlementCommand request, CancellationToken cancellationToken)
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

            if (entity.Type == RecipeEntitlementType.AUTHOR)
            {
                throw new CanNotChangeRecipeAuthorException(entity.RecipeId);
            }

            var recipeAuthor = await _context.RecipeEntitlements
                    .SingleOrDefaultAsync(x => x.RecipeId == entity.RecipeId && x.Type == RecipeEntitlementType.AUTHOR, cancellationToken);

            if (recipeAuthor == null)
            {
                throw new RecipeHasNoAuthorException(entity.RecipeId);
            }

            entity.Type = (RecipeEntitlementType)Enum.Parse(typeof(RecipeEntitlementType), request.Type.Trim().ToUpper());
            entity.LastModifiedBy = request.GrantedBy;


            if (!string.Equals(recipeAuthor.CreatedBy, entity.LastModifiedBy, StringComparison.OrdinalIgnoreCase))
            {
                throw new CanNotModifyRecipeEntitlementException(entity.RecipeId, entity.UserId);
            }

            _context.RecipeEntitlements.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            var response = _mapper.Map<UpdateRecipeEntitlementResponse>(entity);
            return response;
        }
    }
}