using System;
using CulinaCloud.Analytics.Application.Models;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;

namespace CulinaCloud.Analytics.Application.Recommendations.Queries.GetPersonalRecipeRecommendations
{
    public class PersonalRecipeRecommendationsResponse : IMapFrom<RecommendationResult>
    {
        public Guid RecipeId { get; set; }
        public decimal PopularityScore { get; set; }
        public float? PredictedScore { get; set; }
    }
}