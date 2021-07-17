using System;
using System.Collections.Generic;
using CulinaCloud.BuildingBlocks.Common;

namespace CulinaCloud.CookBook.Domain.Entities
{
    public class Recipe : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int EstimatedMinutes { get; set; }
        public string Serves { get; set; }
        public string Yield { get; set; }
        public int NumberOfSteps { get; set; }
        public int NumberOfIngredients { get; set; }
        
        // Required
        public IList<RecipeStep> Steps { get; private set; } = new List<RecipeStep>();
        public IList<RecipeIngredient> Ingredients { get; private set; } = new List<RecipeIngredient>();

        // Not Required
        public IList<RecipeImage> Images { get; private set; } = new List<RecipeImage>();
        public RecipeNutrition Nutrition { get; set; }
        public IList<RecipeMetadata> Metadata { get; private set; } = new List<RecipeMetadata>();
        public IList<RecipeTag> Tags { get; private set; } = new List<RecipeTag>();
    }
}
