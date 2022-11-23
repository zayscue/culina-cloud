namespace CulinaCloud.Web.BFF.APIGateway.Models
{
    public record ApplicationStatisticsDto
    {
        public ApplicationMetricsDto ApplicationMetrics { get; set; } = new();
        public RankedApplicationStatisticsDto ApplicationRankedStatistics { get; set; } = new();
        public HistoricalApplicationStatisticsDto HistoricalApplicationStatistics { get; set; } = new();
    }

    public record ApplicationMetricsDto
    {
        public long TotalActiveApplicationUsers { get; set; }
        public long TotalRecipes { get; set; }
        public long TotalReviews { get; set; }
    }

    public record RankedApplicationStatisticsDto
    {
        public ICollection<RecipePopularityRankingDto> MostPopularRecipes { get; set; } = 
            new List<RecipePopularityRankingDto>();
        public ICollection<TagPopularityRankingDto> MostPopularTags { get; set; } = 
            new List<TagPopularityRankingDto>();
        public ICollection<IngredientPopularityRankingDto> MostPopularIngredients { get; set; } = 
            new List<IngredientPopularityRankingDto>();
    }

    public record RecipePopularityRankingDto
    {
        public string? RecipeName { get; set; }
        public int RatingCount { get; set; }
        public int RatingSum { get; set; }
        public decimal RatingAverage { get; set; }
        public decimal RatingWeightedAverage { get; set; }
    }

    public record TagPopularityRankingDto
    {
        public string? TagName { get; set; }
        public long TotalRecipeTags { get; set; }
    }

    public record IngredientPopularityRankingDto
    {
        public string? IngredientName { get; set; }
        public long TotalIngredientReferences { get; set; }
    }

    public record HistoricalApplicationStatisticsDto
    {
        public ICollection<ApplicationUsersDailyStatisticsDto> ApplicationUsersDailyStatistics { get; set; } =
            new List<ApplicationUsersDailyStatisticsDto>();

        public ICollection<RecipeDailyStatisticsDto> RecipeDailyStatistics { get; set; } =
            new List<RecipeDailyStatisticsDto>();

        public ICollection<ReviewDailyStatisticsDto> ReviewDailyStatistics { get; set; } =
            new List<ReviewDailyStatisticsDto>();
    }

    public record ApplicationUsersDailyStatisticsDto
    {
        public int Logins { get; set; }
        public int SignUps { get; set; }
        public DateTime Date { get; set; }
    }

    public record RecipeDailyStatisticsDto
    {
        public int NewRecipes { get; set; }
        public DateTime Date { get; set; }
    }

    public record ReviewDailyStatisticsDto
    {
        public int NewReviews { get; set; }
        public DateTime Date { get; set; }
    }
}
