using System;
using System.Collections.Generic;

namespace CulinaCloud.Analytics.Domain.Entities
{
    public record Statistics
    {
        public RecipeCreationStatistics NumberOfRecipesCreated { get; set; }
        public List<RecipePopularityStatistic> MostPopularRecipes { get; set; } = new();
        public List<RecentRecipeStatistic> RecentRecipes { get; set; } = new();
    }

    public record RecentRecipeStatistic
    {
        public Guid RecipeId { get; set; }
        public DateTime Created { get; set; }
    }

    public record RecipePopularityStatistic
    {
        public Guid RecipeId { get; set; }
        public int RatingCount { get; set; }
        public int RatingSum { get; set; }
        public decimal RatingAverage { get; set; }
        public decimal RatingWeightedAverage { get; set; }
    }

    public record RecipeCreationStatistics
    { 
        public int InTheLastWeek { get; set; }
        public int InTheLastMonth { get; set; }
        public int InTheLastYear { get; set; }
    }
}
