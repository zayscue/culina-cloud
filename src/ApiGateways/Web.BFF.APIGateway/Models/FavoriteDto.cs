namespace CulinaCloud.Web.BFF.APIGateway.Models;

public record FavoriteDto
{
    public Guid RecipeId { get; set; }
    public string? UserId { get; set; }
    public string? CreatedBy { get; set; }
}