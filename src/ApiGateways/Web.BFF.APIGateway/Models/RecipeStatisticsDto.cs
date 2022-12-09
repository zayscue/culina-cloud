namespace CulinaCloud.Web.BFF.APIGateway.Models
{
    public record RecipeStatisticsDto
    {
        public long TotalRecipes { get; set; }
        public ICollection<TagPopularityRankingDto> MostPopularTags { get; set; } =
            new List<TagPopularityRankingDto>();

        public ICollection<IngredientPopularityRankingDto> MostPopularIngredients { get; set; } =
            new List<IngredientPopularityRankingDto>();

        public ICollection<DailyRecipeStatisticsDto> DailyRecipeStatistics { get; set; } = new
            List<DailyRecipeStatisticsDto>();
    }

    public record TagPopularityRankingDto
    {
        public string TagName { get; set; } = default!;
        public long TotalRecipeTags { get; set; }
    }

    public record IngredientPopularityRankingDto
    {
        public string IngredientName { get; set; } = default!;
        public long TotalIngredientReferences { get; set; }
    }

    public record DailyRecipeStatisticsDto
    {
        public int NewRecipes { get; set; }
        public DateTime Date { get; set; }
    }
}
