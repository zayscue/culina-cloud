using System;

namespace CulinaCloud.Analytics.Application.Models
{
    public class RecommendationResult
    {
        public Guid RecipeId { get; set; }
        public decimal PopularityScore { get; set; }
        public float? PredictedScore { get; set; }
    }
}