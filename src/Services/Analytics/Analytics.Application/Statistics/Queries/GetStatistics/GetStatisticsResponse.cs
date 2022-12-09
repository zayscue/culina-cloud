using CulinaCloud.Analytics.Domain.Entities;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using System;
using System.Collections.Generic;

namespace CulinaCloud.Analytics.Application.Statistics.Queries.GetStatistics
{
    public class GetStatisticsResponse : IMapFrom<Domain.Entities.Statistics>
    {
        public long TotalHistoricalReviews { get; set; }
        public long TotalHistoricalRecipes { get; set; }

        public ICollection<RecipePopularityRanking> MostPopularRecipes { get; set; } = new List<RecipePopularityRanking>();
    }
}
