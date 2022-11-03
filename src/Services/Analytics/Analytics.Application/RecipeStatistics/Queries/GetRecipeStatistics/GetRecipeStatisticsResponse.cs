using CulinaCloud.Analytics.Domain.Entities;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using System;
using System.Collections.Generic;

namespace CulinaCloud.Analytics.Application.RecipeStatistics.Queries.GetRecipeStatistics
{
    public class GetRecipeStatisticsResponse : IMapFrom<Statistics>
    {
        public RecipeCreationStatisticsResponse NumberOfRecipesCreated { get; set; }
        public List<RecipePopularityStatisticResponse> MostPopularRecipes { get; set; } = new();
        public List<RecentRecipeStatisticResponse> RecentRecipes { get; set; } = new();
    }

    public record RecentRecipeStatisticResponse : IMapFrom<RecentRecipeStatistic>
    {
        public Guid RecipeId { get; set; }
        public DateTime Created { get; set; }
    }

    public record RecipePopularityStatisticResponse : IMapFrom<RecipePopularityStatistic>
    {
        public Guid RecipeId { get; set; }
        public int RatingCount { get; set; }
        public int RatingSum { get; set; }
        public decimal RatingAverage { get; set; }
        public decimal RatingWeightedAverage { get; set; }
    }

    public record RecipeCreationStatisticsResponse : IMapFrom<RecipeCreationStatistics>
    {
        public int InTheLastWeek { get; set; }
        public int InTheLastMonth { get; set; }
        public int InTheLastYear { get; set; }
    }
}
