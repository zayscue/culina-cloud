using System;
using System.Collections.Generic;

namespace CulinaCloud.CookBook.Domain.Entities
{
    public record Statistics
    {
        public long TotalRecipes { get; set; }

        public ICollection<TagPopularityRanking> MostPopularTags { get; set; } = 
            new List<TagPopularityRanking>();

        public ICollection<IngredientPopularityRanking> MostPopularIngredients { get; set; } =
            new List<IngredientPopularityRanking>();

        public ICollection<DailyRecipeStatistics> DailyRecipeStatistics { get; set; } = new 
            List<DailyRecipeStatistics>();
    }

    public record TagPopularityRanking
    {
        public string TagName { get; set; }
        public long TotalRecipeTags { get; set; }
    }

    public record IngredientPopularityRanking
    {
        public string IngredientName { get; set; }
        public long TotalIngredientReferences { get; set; }
    }

    public record DailyRecipeStatistics
    {
        public int NewRecipes { get; set; }
        public DateTime Date { get; set; }
    }
}
