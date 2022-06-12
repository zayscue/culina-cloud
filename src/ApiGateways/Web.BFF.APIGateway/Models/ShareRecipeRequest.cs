namespace CulinaCloud.Web.BFF.APIGateway.Models;

public record ShareRecipeRequest
{
    public Guid? Id { get; set; }
    public Guid RecipeId { get; set; }
    public string UserEmail { get; set; } = default!;
    public string Type { get; set; } = default!;
}
