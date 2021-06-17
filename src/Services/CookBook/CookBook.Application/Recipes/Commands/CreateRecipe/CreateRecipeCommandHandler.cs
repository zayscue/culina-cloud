using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Culina.CookBook.Application.Common.Interfaces;
using Culina.CookBook.Domain.Entities;
using Culina.CookBook.Domain.Events;
using CulinaCloud.BuildingBlocks.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Culina.CookBook.Application.Recipes.Commands.CreateRecipe
{
    public class CreateRecipeCommandHandler : IRequestHandler<CreateRecipeCommand, CreateRecipeResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;
        private readonly IAggregateEventService _aggregateEventService;
        private readonly IMapper _mapper;

        public CreateRecipeCommandHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService,
            IDateTime dateTime,
            IAggregateEventService aggregateEventService,
            IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _dateTime = dateTime;
            _aggregateEventService = aggregateEventService;
            _mapper = mapper;
        }

        public async Task<CreateRecipeResponse> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
        {
            var now = _dateTime.Now;
            var currentUserId = _currentUserService.UserId;
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
                Created = now,
                CreatedBy = currentUserId
            };
            var @event = new RecipeCreatedEvent
            {
                AggregateId = entity.Id,
                Details = "A new recipe was created using the POST \"/cookbook/recipe\" API.",
                Occurred = entity.Created,
                RaisedBy = entity.CreatedBy,
                Data = {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                    EstimatedMinutes = entity.EstimatedMinutes,
                    Serves = entity.Serves,
                    Yield = entity.Yield,
                    NumberOfSteps = entity.NumberOfSteps,
                    NumberOfIngredients = entity.NumberOfIngredients,
                    Steps = request.Steps,
                    Ingredients = request.Ingredients?.Select(x => new RecipeIngredientCreated {
                        Quantity = x.Quantity,
                        Part = x.Part,
                        Type = x.Type
                    }).ToList() ?? null,
                    ImageUrls = request.ImageUrls,
                    Nutrition = request.Nutrition != null
                        ? new RecipeNutritionCreated
                        {
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
                            TotalCarbohydratesPdv = request.Nutrition.TotalCarbohydratesPdv
                        }
                        : null,
                    Metadata = request.Metadata?.Select(x => new RecipeMetadataCreated {
                        Type = x.Type,
                        Value = x.Value
                    }).ToList() ?? null,
                    Tags = request.Tags
                }
            };
            var tagCreatedEvents = new List<TagCreatedEvent>();
            var ingredientCreatedEvents = new List<IngredientCreatedEvent>();

            for (var i = 0; i < request.Steps.Count; i++)
            {
                var step = request.Steps[i];
                entity.Steps.Add(new RecipeStep
                {
                    RecipeId = recipeId,
                    Order = i + 1,
                    Instruction = step,
                    Created = now,
                    CreatedBy = currentUserId
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
                                Created = now,
                                CreatedBy = currentUserId
                            };
                        entity.Ingredients.Add(new RecipeIngredient
                        {
                            RecipeId = recipeId,
                            Id = recipeIngredientId,
                            Quantity = recipeIngredient.Quantity,
                            Part = recipeIngredient.Part,
                            IngredientId = ingredientId,
                            Created = now,
                            CreatedBy = currentUserId,
                            Ingredient = newIngredient
                        });
                        if (uniqueIngredients.ContainsKey(recipeIngredient.Type)) continue;
                        ingredientCreatedEvents.Add(new IngredientCreatedEvent
                        {
                            AggregateId = ingredientId,
                            Details = "A new ingredient was created using the POST \"/cookbook/recipe\" API.",
                            Occurred = now,
                            RaisedBy = currentUserId,
                            Data = {
                                Id = ingredientId,
                                IngredientName = recipeIngredient.Type
                            }
                        });
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
                            Created = now,
                            CreatedBy = currentUserId
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
                        Created = now,
                        CreatedBy = currentUserId
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
                        Created = now,
                        CreatedBy = currentUserId
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
                            Created = now,
                            CreatedBy = currentUserId,
                            Image = new Image
                            {
                                Id = imageId,
                                Url = imageUrl,
                                Created = now,
                                CreatedBy = currentUserId
                            }
                        });
                    }
                    else
                    {
                        entity.Images.Add(new RecipeImage
                        {
                            RecipeId = recipeId,
                            ImageId = image.Id,
                            Created = now,
                            CreatedBy = currentUserId
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
                            Created = now,
                            CreatedBy = currentUserId,
                            Tag = new Tag
                            {
                                Id = tagId,
                                TagName = tagName,
                                Created = now,
                                CreatedBy = currentUserId
                            }
                        });
                        tagCreatedEvents.Add(new TagCreatedEvent
                        {
                            AggregateId = tagId,
                            Details = "A new recipe tag was created using the POST \"/cookbook/recipe\" API.",
                            Occurred = now,
                            RaisedBy = currentUserId,
                            Data = {
                                Id = tagId,
                                TagName = tagName
                            }
                        });
                    }
                    else
                    {
                        entity.Tags.Add(new RecipeTag()
                        {
                            RecipeId = recipeId,
                            TagId = tag.Id,
                            Created = now,
                            CreatedBy = currentUserId
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
                    Created = now,
                    CreatedBy = currentUserId
                };
            }

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            await _context.EventOutbox.AddAsync(new AggregateEventEntity(@event.ToAggregateEvent()),
                    cancellationToken);
            foreach (var ingredientCreatedEvent in ingredientCreatedEvents)
            {
                await _context.EventOutbox.AddAsync(new AggregateEventEntity(ingredientCreatedEvent.ToAggregateEvent()),
                    cancellationToken);
            }
            foreach (var tagCreatedEvent in tagCreatedEvents)
            {
                await _context.EventOutbox.AddAsync(new AggregateEventEntity(tagCreatedEvent.ToAggregateEvent()),
                    cancellationToken);
            }
            await _context.Recipes.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);


            await _aggregateEventService.Publish(@event, cancellationToken);
            foreach (var ingredientCreatedEvent in ingredientCreatedEvents)
            {
                await _aggregateEventService.Publish(ingredientCreatedEvent, cancellationToken);
            }
            foreach (var tagCreatedEvent in tagCreatedEvents)
            {
                await _aggregateEventService.Publish(tagCreatedEvent, cancellationToken);
            }

            var response = _mapper.Map<CreateRecipeResponse>(entity);

            return response;
        }
    }
}