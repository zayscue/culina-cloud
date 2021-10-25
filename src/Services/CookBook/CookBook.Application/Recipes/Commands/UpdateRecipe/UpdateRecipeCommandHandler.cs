using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using CulinaCloud.CookBook.Domain.Entities;
using MediatR;

namespace CulinaCloud.CookBook.Application.Recipes.Commands.UpdateRecipe
{
    public class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommand, UpdateRecipeResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateRecipeCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UpdateRecipeResponse> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Recipes.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Recipe), request.Id);
            }

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.EstimatedMinutes = request.EstimatedMinutes;
            entity.Serves = request.Serves;
            entity.Yield = request.Yield;
            entity.LastModifiedBy = request.LastModifiedBy;

            await _context.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<UpdateRecipeResponse>(entity);

            return response;
        }
    }
}