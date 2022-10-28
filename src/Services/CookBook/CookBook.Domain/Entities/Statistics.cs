using System.Collections.Generic;

namespace CulinaCloud.CookBook.Domain.Entities
{
    public record Statistics
    {
        public RecipeStatistics RecipeStatistics { get; set; } = new();
        public List<RecipeIngredientStatistic> IngredientStatistics { get; set; } = new();
        public List<RecipeTagStatistic> TagStatistics { get; set; } = new();
    }

    public record RecipeStatistics
    {
        public int RecipeCount { get; set; }
    }

    public record RecipeIngredientStatistic
    {
        public int RecipeCount { get; set; }
        public string IngredientName { get; set; }
    }

    public record RecipeTagStatistic
    {
        public int RecipeCount { get; set; }
        public string TagName { get; set; }
    }
}
