using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using CulinaCloud.CookBook.Domain.Entities;
using MediatR;

namespace CulinaCloud.CookBook.Application.RecipeNutritionFacts.Commands.UpdateRecipeNutrition
{
    public class UpdateRecipeNutritionCommand : IRequest<UpdateRecipeNutritionResponse>
    {
        public Guid RecipeId { get; set; }
        public string ServingSize { get; set; }
        public int ServingsPerRecipe { get; set; }
        public decimal Calories { get; set; }
        public decimal CaloriesFromFat { get; set; }
        public decimal CaloriesFromFatPdv { get; set; }
        public decimal TotalFat { get; set; }
        public decimal TotalFatPdv { get; set; }
        public decimal SaturatedFat { get; set; }
        public decimal SaturatedFatPdv { get; set; }
        public decimal Cholesterol { get; set; }
        public decimal CholesterolPdv { get; set; }
        public decimal DietaryFiber { get; set; }
        public decimal DietaryFiberPdv { get; set; }
        public decimal Sugar { get; set; }
        public decimal SugarPdv { get; set; }
        public decimal Sodium { get; set; }
        public decimal SodiumPdv { get; set; }
        public decimal Protein { get; set; }
        public decimal ProteinPdv { get; set; }
        public decimal TotalCarbohydrates { get; set; }
        public decimal TotalCarbohydratesPdv { get; set; }
        public string LastModifiedBy { get; set; }
    }

    public class UpdateRecipeNutritionCommandHandler : IRequestHandler<UpdateRecipeNutritionCommand, UpdateRecipeNutritionResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateRecipeNutritionCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UpdateRecipeNutritionResponse> Handle(UpdateRecipeNutritionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.RecipeNutrition.FindAsync(request.RecipeId);
            if (entity == null)
            {
                throw new NotFoundException(nameof(RecipeNutrition), request.RecipeId);
            }

            entity.ServingSize = request.ServingSize;
            entity.ServingsPerRecipe = request.ServingsPerRecipe;
            entity.Calories = request.Calories;
            entity.CaloriesFromFat = request.CaloriesFromFat;
            entity.CaloriesFromFatPdv = request.CaloriesFromFatPdv;
            entity.TotalFat = request.TotalFat;
            entity.TotalFatPdv = request.TotalFatPdv;
            entity.SaturatedFat = request.SaturatedFat;
            entity.SaturatedFatPdv = request.SaturatedFatPdv;
            entity.Cholesterol = request.Cholesterol;
            entity.CholesterolPdv = request.CholesterolPdv;
            entity.DietaryFiber = request.DietaryFiber;
            entity.DietaryFiberPdv = request.DietaryFiberPdv;
            entity.Sugar = request.Sugar;
            entity.SugarPdv = request.SugarPdv;
            entity.Sodium = request.Sodium;
            entity.SodiumPdv = request.SodiumPdv;
            entity.Protein = request.Protein;
            entity.ProteinPdv = request.ProteinPdv;
            entity.TotalCarbohydrates = request.TotalCarbohydrates;
            entity.TotalCarbohydratesPdv = request.TotalCarbohydratesPdv;
            entity.LastModifiedBy = request.LastModifiedBy;

            await _context.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<UpdateRecipeNutritionResponse>(entity);

            return response;
        }
    }
}