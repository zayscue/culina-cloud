using System;
using System.Collections.Generic;
using CulinaCloud.BuildingBlocks.Common;

namespace CulinaCloud.CookBook.Domain.Events
{
    public class RecipeIngredientCreated
    {
        public string Quantity { get; set; }
        public string Part { get; set; }
        public string Type { get; set; }
    }

    public class RecipeNutritionCreated
    {
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
    }

    public class RecipeMetadataCreated
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public interface IRecipeCreated
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        int EstimatedMinutes { get; set; }
        string Serves { get; set; }
        string Yield { get; set; }
        int NumberOfSteps { get; set; }
        int NumberOfIngredients { get; set; }
        IList<string> Steps { get; set;}
        IList<RecipeIngredientCreated> Ingredients { get; set; }
        IList<string> ImageUrls { get; set; }
        RecipeNutritionCreated Nutrition { get; set; }
        IList<RecipeMetadataCreated> Metadata { get; set; }
        IList<string> Tags { get; set; }
    }

    sealed class RecipeCreated : IRecipeCreated
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int EstimatedMinutes { get; set; }
        public string Serves { get; set; }
        public string Yield { get; set; }
        public int NumberOfSteps { get; set; }
        public int NumberOfIngredients { get; set; }
        public IList<string> Steps { get; set; }
        public IList<RecipeIngredientCreated> Ingredients { get; set; }
        public IList<string> ImageUrls { get; set; }
        public RecipeNutritionCreated Nutrition { get; set; }
        public IList<RecipeMetadataCreated> Metadata { get; set; }
        public IList<string> Tags { get; set; }
    }

  public sealed class RecipeCreatedEvent : AggregateEventBase<IRecipeCreated>
  {
      public override string AggregateType => "Recipe";

      public RecipeCreatedEvent()
      {
          Data = new RecipeCreated();
      }
  }
}