using System;
using MediatR;

namespace CulinaCloud.CookBook.Application.RecipeNutritionFacts.Commands.CreateRecipeNutrition
{
    public class CreateRecipeNutritionCommand : IRequest<CreateRecipeNutritionResponse>
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
        public string CreatedBy { get; set; }
    }
}