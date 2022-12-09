namespace CulinaCloud.Web.BFF.APIGateway.Models;

public record RecipeRecommendationDto
{
    public Guid RecipeId { get; set; }
    public decimal PopularityScore { get; set; }
    public float? PredictedScore { get; set; }
}