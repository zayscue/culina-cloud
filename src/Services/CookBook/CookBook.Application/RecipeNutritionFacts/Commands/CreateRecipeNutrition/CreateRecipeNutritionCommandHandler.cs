using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using CulinaCloud.CookBook.Domain.Entities;
using MediatR;

namespace CulinaCloud.CookBook.Application.RecipeNutritionFacts.Commands.CreateRecipeNutrition
{
    public class CreateRecipeNutritionCommandHandler : IRequestHandler<CreateRecipeNutritionCommand, CreateRecipeNutritionResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateRecipeNutritionCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CreateRecipeNutritionResponse> Handle(CreateRecipeNutritionCommand request, CancellationToken cancellationToken)
        {
            var recipe = await _context.Recipes.FindAsync(request.RecipeId);

            if (recipe == null)
            {
                throw new NotFoundException(nameof(Recipe), request.RecipeId);
            }

            var entity = new RecipeNutrition
            {
                RecipeId = request.RecipeId,
                ServingSize = request.ServingSize,
                ServingsPerRecipe = request.ServingsPerRecipe,
                Calories = request.Calories,
                CaloriesFromFat = request.CaloriesFromFat,
                CaloriesFromFatPdv = request.CaloriesFromFatPdv,
                TotalFat = request.TotalFat,
                TotalFatPdv = request.TotalFatPdv,
                SaturatedFat = request.SaturatedFat,
                SaturatedFatPdv = request.SaturatedFatPdv,
                Cholesterol = request.Cholesterol,
                CholesterolPdv = request.CholesterolPdv,
                DietaryFiber = request.DietaryFiber,
                DietaryFiberPdv = request.DietaryFiberPdv,
                Sugar = request.Sugar,
                SugarPdv = request.SugarPdv,
                Sodium = request.Sodium,
                SodiumPdv = request.SodiumPdv,
                Protein = request.Protein,
                ProteinPdv = request.ProteinPdv,
                TotalCarbohydrates = request.TotalCarbohydrates,
                TotalCarbohydratesPdv = request.TotalCarbohydratesPdv,
                CreatedBy = request.CreatedBy,
                Recipe = recipe
            };

            await _context.RecipeNutrition.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<CreateRecipeNutritionResponse>(entity);

            return response;
        }
    }
}