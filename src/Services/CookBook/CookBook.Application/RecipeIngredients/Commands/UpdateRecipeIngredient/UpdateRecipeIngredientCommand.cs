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

namespace CulinaCloud.CookBook.Application.RecipeIngredients.Commands.UpdateRecipeIngredient
{
    public class UpdateRecipeIngredientCommand : IRequest<UpdateRecipeIngredientResponse>
    {
        public Guid RecipeId { get; set; }
        public Guid RecipeIngredientId { get; set; }
        public string Quantity { get; set; }
        public string Part { get; set; }
        public string IngredientName { get; set; }
        public string LastModifiedBy { get; set; }
    }

    public class UpdateRecipeIngredientCommandHandler : IRequestHandler<UpdateRecipeIngredientCommand, UpdateRecipeIngredientResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateRecipeIngredientCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UpdateRecipeIngredientResponse> Handle(UpdateRecipeIngredientCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.RecipeIngredients
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

            Ingredient ingredient = null;
            Guid? ingredientId = null;
            if (!string.IsNullOrWhiteSpace(request.IngredientName)
                && !string.Equals(request.IngredientName.ToLower(), entity.Ingredient?.IngredientName?.ToLower()))
            {
                ingredient = await _context.Ingredients
                    .FirstOrDefaultAsync(x => x.IngredientName.ToLower().Equals(request.IngredientName.ToLower()), cancellationToken);
                if (ingredient == null)
                {
                    ingredient = new Ingredient
                    {
                        Id = Guid.NewGuid(),
                        IngredientName = request.IngredientName,
                        CreatedBy = request.LastModifiedBy
                    };
                    await _context.Ingredients.AddAsync(ingredient, cancellationToken);
                }
                ingredientId = ingredient != null ? ingredient.Id : null;
            }

            entity.Quantity = request.Quantity;
            entity.Part = request.Part;
            entity.LastModifiedBy = request.LastModifiedBy;
            if (ingredient != null)
            {
                entity.IngredientId = ingredientId;
                entity.Ingredient = ingredient;
            }
            _context.RecipeIngredients.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<UpdateRecipeIngredientResponse>(entity);

            return response;
        }
    }
}