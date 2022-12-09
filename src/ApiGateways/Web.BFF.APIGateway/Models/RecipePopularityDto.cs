namespace CulinaCloud.Web.BFF.APIGateway.Models;

public record RecipePopularityDto
{
    public Guid RecipeId { get; set; }
    public string? Submitted { get; set; }
    public int RatingCount { get; set; }
    public int RatingSum { get; set; }
    public decimal RatingAverage { get; set; }
    public decimal RatingWeightedAverage { get; set; }
    public string? CreatedBy { get; set; }
    public string? LastModifiedBy { get; set; }
}