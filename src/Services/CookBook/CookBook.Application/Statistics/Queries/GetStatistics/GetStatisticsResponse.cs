using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.CookBook.Domain.Entities;
using System.Collections.Generic;

namespace CulinaCloud.CookBook.Application.Statistics.Queries.GetStatistics
{
    public class GetStatisticsResponse : IMapFrom<Domain.Entities.Statistics>
    {
        public long TotalRecipes { get; set; }

        public ICollection<TagPopularityRanking> MostPopularTags { get; set; } =
            new List<TagPopularityRanking>();

        public ICollection<IngredientPopularityRanking> MostPopularIngredients { get; set; } =
            new List<IngredientPopularityRanking>();

        public ICollection<DailyRecipeStatistics> DailyRecipeStatistics { get; set; } = new
            List<DailyRecipeStatistics>();
    }
}
