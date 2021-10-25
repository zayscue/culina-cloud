using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using CulinaCloud.CookBook.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.CookBook.Application.Recipes.Commands.CreateRecipe
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
            var createdBy = request.CreatedBy;
            var recipeId = request.Id ?? Guid.NewGuid();
            var entity = new Recipe
            {
                Id = recipeId,
                Name = request.Name,
                Description = request.Description,
                EstimatedMinutes = request.EstimatedMinutes,
                Serves = request.Serves,
                Yield = request.Yield,
                NumberOfSteps = request.Steps.Count,
                NumberOfIngredients = request.Ingredients.Count,
                CreatedBy = createdBy
            };

            for (var i = 0; i < request.Steps.Count; i++)
            {
                var step = request.Steps[i];
                entity.Steps.Add(new RecipeStep
                {
                    RecipeId = recipeId,
                    Order = i + 1,
                    Instruction = step,
                    CreatedBy = createdBy
                });
            }

            var uniqueIngredients = new Dictionary<string, Ingredient>();
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
                        var newIngredient = uniqueIngredients.ContainsKey(recipeIngredient.Type) 
                            ? uniqueIngredients[recipeIngredient.Type]
                            : new Ingredient
                            {
                                Id = ingredientId,
                                IngredientName = recipeIngredient.Type,
                                CreatedBy = createdBy
                            };
                        entity.Ingredients.Add(new RecipeIngredient
                        {
                            RecipeId = recipeId,
                            Id = recipeIngredientId,
                            Quantity = recipeIngredient.Quantity,
                            Part = recipeIngredient.Part,
                            IngredientId = ingredientId,
                            CreatedBy = createdBy,
                            Ingredient = newIngredient
                        });
                        if (uniqueIngredients.ContainsKey(recipeIngredient.Type)) continue;
                        uniqueIngredients.Add(recipeIngredient.Type, newIngredient);
                    }
                    else
                    {
                        entity.Ingredients.Add(new RecipeIngredient
                        {
                            RecipeId = recipeId,
                            Id = recipeIngredientId,
                            Quantity = recipeIngredient.Quantity,
                            Part = recipeIngredient.Part,
                            IngredientId = ingredient.Id,
                            Ingredient = ingredient,
                            CreatedBy = createdBy
                        });
                    }
                }
                else
                {
                    entity.Ingredients.Add(new RecipeIngredient
                    {
                        RecipeId = recipeId,
                        Id = recipeIngredientId,
                        Quantity = recipeIngredient.Quantity,
                        Part = recipeIngredient.Part,
                        CreatedBy = createdBy
                    });
                }
            }

            if (request.Metadata != null)
            {
                foreach (var metadata in request.Metadata)
                {
                    entity.Metadata.Add(new RecipeMetadata
                    {
                        RecipeId = recipeId,
                        Type = metadata.Type,
                        Value = metadata.Value,
                        CreatedBy = createdBy
                    });
                }
            }

            if (request.ImageUrls != null)
            {
                foreach (var imageUrl in request.ImageUrls)
                {
                    var image = await _context.Images.FirstOrDefaultAsync(x =>
                            x.Url.Equals(imageUrl),
                        cancellationToken);
                    if (image == null)
                    {
                        var imageId = Guid.NewGuid();
                        entity.Images.Add(new RecipeImage
                        {
                            RecipeId = recipeId,
                            ImageId = imageId,
                            CreatedBy = createdBy,
                            Image = new Image
                            {
                                Id = imageId,
                                Url = imageUrl,
                                CreatedBy = createdBy
                            }
                        });
                    }
                    else
                    {
                        entity.Images.Add(new RecipeImage
                        {
                            RecipeId = recipeId,
                            ImageId = image.Id,
                            CreatedBy = createdBy
                        });
                    }
                }
            }

            if (request.Tags != null)
            {
                foreach (var tagName in request.Tags)
                {
                    var tag = await _context.Tags.FirstOrDefaultAsync(x =>
                            x.TagName.Equals(tagName),
                        cancellationToken);
                    if (tag == null)
                    {
                        var tagId = Guid.NewGuid();
                        entity.Tags.Add(new RecipeTag
                        {
                            RecipeId = recipeId,
                            TagId = tagId,
                            CreatedBy = createdBy,
                            Tag = new Tag
                            {
                                Id = tagId,
                                TagName = tagName,
                                CreatedBy = createdBy
                            }
                        });
                    }
                    else
                    {
                        entity.Tags.Add(new RecipeTag()
                        {
                            RecipeId = recipeId,
                            TagId = tag.Id,
                            CreatedBy = createdBy
                        });
                    }
                }
            }

            if (request.Nutrition != null)
            {
                entity.Nutrition = new RecipeNutrition
                {
                    RecipeId = recipeId,
                    ServingSize = request.Nutrition.ServingSize,
                    ServingsPerRecipe = request.Nutrition.ServingsPerRecipe,
                    Calories = request.Nutrition.Calories,
                    CaloriesFromFat = request.Nutrition.CaloriesFromFat,
                    CaloriesFromFatPdv = request.Nutrition.CaloriesFromFatPdv,
                    TotalFat = request.Nutrition.TotalFat,
                    TotalFatPdv = request.Nutrition.TotalFatPdv,
                    SaturatedFat = request.Nutrition.SaturatedFat,
                    SaturatedFatPdv = request.Nutrition.SaturatedFatPdv,
                    Cholesterol = request.Nutrition.Cholesterol,
                    CholesterolPdv = request.Nutrition.CholesterolPdv,
                    DietaryFiber = request.Nutrition.DietaryFiber,
                    DietaryFiberPdv = request.Nutrition.DietaryFiberPdv,
                    Sugar = request.Nutrition.Sugar,
                    SugarPdv = request.Nutrition.SugarPdv,
                    Sodium = request.Nutrition.Sodium,
                    SodiumPdv = request.Nutrition.SodiumPdv,
                    Protein = request.Nutrition.Protein,
                    ProteinPdv = request.Nutrition.ProteinPdv,
                    TotalCarbohydrates = request.Nutrition.TotalCarbohydrates,
                    TotalCarbohydratesPdv = request.Nutrition.TotalCarbohydratesPdv,
                    CreatedBy = createdBy
                };
            }

            await _context.Recipes.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<CreateRecipeResponse>(entity);

            return response;
        }
    }
}