using System;
using System.Collections.Generic;

namespace CulinaCloud.Analytics.Domain.Entities
{
    public record Statistics
    {
        public long TotalHistoricalReviews { get; set; }
        public long TotalHistoricalRecipes { get; set; }

        public ICollection<RecipePopularityRanking> MostPopularRecipes { get; set; } = new List<RecipePopularityRanking>();
    }

    public record RecipePopularityRanking
    {
        public Guid RecipeId { get; set; }
        public int RatingCount { get; set; }
        public int RatingSum { get; set; }
        public decimal RatingAverage { get; set; }
        public decimal RatingWeightedAverage { get; set; }
    }
}
