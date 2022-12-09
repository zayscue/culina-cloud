using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using CulinaCloud.CookBook.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.CookBook.Application.RecipeIngredients.Commands.DeleteRecipeIngredient
{
    public class DeleteRecipeIngredientCommand : IRequest<DeleteRecipeIngredientResponse>
    {
        public Guid RecipeId { get; set; }
        public Guid RecipeIngredientId { get; set; }
    }

    public class DeleteRecipeIngredientCommandHandler : IRequestHandler<DeleteRecipeIngredientCommand, DeleteRecipeIngredientResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteRecipeIngredientCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<DeleteRecipeIngredientResponse> Handle(DeleteRecipeIngredientCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.RecipeIngredients
                .AsNoTracking()
                .Include(x => x.Ingredient)
                .SingleOrDefaultAsync(x => x.RecipeId == request.RecipeId
                    && x.Id == request.RecipeIngredientId, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(
                    nameof(RecipeIngredient),
                    JsonSerializer.Serialize(new
                    {
                        RecipeId = request.RecipeId,
                        RecipeIngredientId = request.RecipeIngredientId
                    })
                );
            }

            _context.RecipeIngredients.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<DeleteRecipeIngredientResponse>(entity);

            return response;
        }
    }
}