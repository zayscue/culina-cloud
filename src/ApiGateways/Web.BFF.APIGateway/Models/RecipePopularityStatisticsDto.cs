namespace CulinaCloud.Web.BFF.APIGateway.Models
{
    public record RecipePopularityStatisticsDto
    {
        public long TotalHistoricalReviews { get; set; }
        public long TotalHistoricalRecipes { get; set; }

        public ICollection<RecipePopularityRankingDto> MostPopularRecipes { get; set; } = new List<RecipePopularityRankingDto>();
    }

    public record RecipePopularityRankingDto
    {
        public Guid RecipeId { get; set; }
        public int RatingCount { get; set; }
        public int RatingSum { get; set; }
        public decimal RatingAverage { get; set; }
        public decimal RatingWeightedAverage { get; set; }
    }
}
