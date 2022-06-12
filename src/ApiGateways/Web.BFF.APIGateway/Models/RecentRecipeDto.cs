namespace CulinaCloud.Web.BFF.APIGateway.Models;

public record RecentRecipeDto
{
    public Guid RecipeId { get; set; }
    public string? Submitted { get; set; }
    public decimal PopularityScore { get; set; }
}
