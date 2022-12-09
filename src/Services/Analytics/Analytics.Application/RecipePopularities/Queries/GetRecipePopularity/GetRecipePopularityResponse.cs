using CulinaCloud.Analytics.Domain.Entities;
using System;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;

namespace CulinaCloud.Analytics.Application.RecipePopularities.Queries.GetRecipePopularity
{
    public class GetRecipePopularityResponse : IMapFrom<RecipePopularity>
    {
        public Guid RecipeId { get; set; }
        public string Submitted { get; set; }
        public int RatingCount { get; set; }
        public int RatingSum { get; set; }
        public decimal RatingAverage { get; set; }
        public decimal RatingWeightedAverage { get; set; }
    }
}
