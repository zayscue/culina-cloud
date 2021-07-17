using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.Analytics.Domain.Entities;
using System;

namespace CulinaCloud.Analytics.Application.RecipePopularities.Commands.CreateRecipePopularity
{
    public class CreateRecipePopularityResponse : IMapFrom<RecipePopularity>
    {
        public Guid RecipeId { get; set; }
        public string Submitted { get; set; }
        public int RatingCount { get; set; }
        public int RatingSum { get; set; }
        public decimal RatingAverage { get; set; }
        public decimal RatingWeightedAverage { get; set; }
    }
}
