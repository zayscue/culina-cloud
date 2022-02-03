using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using CulinaCloud.CookBook.Application.RecipeIngredients.Commands.CreateRecipeIngredient;
using CulinaCloud.CookBook.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.CookBook.Application.RecipeIngredients.Commands.BatchUpdateRecipeIngredients
{
    public class BatchUpdateRecipeIngredientsCommand : IRequest<List<CreateRecipeIngredientResponse>>
    {
        public Guid RecipeId { get; set; }
        public List<CreateRecipeIngredientCommand> Commands { get; set; }
    }


    public class BatchUpdateRecipeIngredientsCommandHandler : IRequestHandler<BatchUpdateRecipeIngredientsCommand,
        List<CreateRecipeIngredientResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BatchUpdateRecipeIngredientsCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<List<CreateRecipeIngredientResponse>> Handle(BatchUpdateRecipeIngredientsCommand request, CancellationToken cancellationToken)
        {
            var recipe = await _context.Recipes
                .AsNoTracking()
                .Include(x => x.Ingredients)
                .SingleOrDefaultAsync(x => x.Id == request.RecipeId, cancellationToken);
            if (recipe == null)
            {
                throw new NotFoundException(nameof(Recipe), request.RecipeId);
            }

            var existingRecipeIngredients = recipe.Ingredients.ToList();
            var commands = request.Commands;
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            _context.RecipeIngredients.RemoveRange(existingRecipeIngredients);
            await _context.SaveChangesAsync(cancellationToken);
            var recipeIngredients = new List<RecipeIngredient>();
            foreach (var command in commands)
            {
                var ingredient = await _context.Ingredients
                    .AsNoTracking()
                    .SingleOrDefaultAsync(
                        x => x.IngredientName.ToLower().Equals(command.IngredientName.Trim().ToLower()), 
                        cancellationToken);
                if (ingredient == null)
                {
                    ingredient = new Ingredient
                    {
                        Id = Guid.NewGuid(),
                        IngredientName = command.IngredientName.Trim(),
                        CreatedBy = command.CreatedBy
                    };
                    await _context.Ingredients.AddAsync(ingredient, cancellationToken);
                }

                var entity = new RecipeIngredient
                {
                    Id = command.RecipeIngredientId == null || command.RecipeIngredientId == Guid.Empty
                        ? Guid.NewGuid()
                        : command.RecipeIngredientId.Value,
                    RecipeId = request.RecipeId,
                    IngredientId = ingredient.Id,
                    Part = command.Part,
                    Quantity = command.Quantity,
                    CreatedBy = command.CreatedBy
                };
                recipeIngredients.Add(entity);
            }
            await _context.RecipeIngredients.AddRangeAsync(recipeIngredients, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            var updatedRecipeIngredientsQuery = _context.RecipeIngredients
                .AsNoTracking()
                .Include(x => x.Ingredient)
                .Where(x => x.RecipeId == request.RecipeId);

            var results = await updatedRecipeIngredientsQuery
                .ProjectToListAsync<CreateRecipeIngredientResponse>(_mapper.ConfigurationProvider, cancellationToken);

            return results;
        }
    }
}