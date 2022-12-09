namespace CulinaCloud.Web.BFF.APIGateway.Models;

public record RecipeSimilarityDto
{
    public Guid RecipeId { get; set; }
    public Guid SimilarRecipeId { get; set; }
    public string? SimilarityType { get; set; }
    public decimal SimilarityScore { get; set; }
    public decimal PopularityScore { get; set; }
}