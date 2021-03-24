using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Culina.CookBook.Application.Common.Interfaces;
using Culina.CookBook.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Culina.CookBook.Application.Recipes.Commands.CreateRecipe
{
    public class CreateRecipeCommandHandler : IRequestHandler<CreateRecipeCommand, CreateRecipeResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        
        public CreateRecipeCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<CreateRecipeResponse> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
        {
            var recipeId = request.Id ?? Guid.NewGuid();
            var entity = new Recipe()
            {
                Id = recipeId,
                Name = request.Name,
                Description = request.Description,
                EstimatedMinutes = request.EstimatedMinutes,
                Serves = request.Serves,
                Yield = request.Yield,
                NumberOfSteps = request.Steps.Count,
                NumberOfIngredients = request.Ingredients.Count
            };

            for (var i = 0; i < request.Steps.Count; i++)
            {
                var step = request.Steps[i];
                entity.Steps.Add(new RecipeStep()
                {
                    RecipeId = recipeId,
                    Order = i + 1,
                    Instruction = step
                });
            }
            
            foreach (var recipeIngredient in request.Ingredients)
            {
                var recipeIngredientId = Guid.NewGuid();
                if (!string.IsNullOrWhiteSpace(recipeIngredient.Type))
                {
                    var ingredient = await _context.Ingredients.FirstOrDefaultAsync(x =>
                            x.IngredientName.ToLower().Equals(recipeIngredient.Type.ToLower()),
                        cancellationToken);
                    if (ingredient == null)
                    {
                        var ingredientId = Guid.NewGuid();
                        entity.Ingredients.Add(new RecipeIngredient()
                        {
                            RecipeId = recipeId,
                            Id = recipeIngredientId,
                            Quantity = recipeIngredient.Quantity,
                            Part = recipeIngredient.Part,
                            IngredientId = ingredientId,
                            Ingredient = new Ingredient()
                            {
                                Id = ingredientId,
                                IngredientName = recipeIngredient.Type
                            }
                        });
                    }
                    else
                    {
                        entity.Ingredients.Add(new RecipeIngredient()
                        {
                            RecipeId = recipeId,
                            Id = recipeIngredientId,
                            Quantity = recipeIngredient.Quantity,
                            Part = recipeIngredient.Part,
                            IngredientId = ingredient.Id,
                            Ingredient = ingredient
                        });
                    }
                }
                else
                {
                    entity.Ingredients.Add(new RecipeIngredient()
                    {
                        RecipeId = recipeId,
                        Id = recipeIngredientId,
                        Quantity = recipeIngredient.Quantity,
                        Part = recipeIngredient.Part
                    });
                }
            }

            if (request.Metadata != null)
            {
                foreach (var metadata in request.Metadata)
                {
                    entity.Metadata.Add(new RecipeMetadata()
                    {
                        RecipeId = recipeId,
                        Type = metadata.Type,
                        Value = metadata.Value
                    });
                }
            }

            await _context.Recipes.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<CreateRecipeResponse>(entity);

            return response;
        }
    }
}