namespace CulinaCloud.Web.BFF.APIGateway.Models;

public record RecipeEntitlementDto
{
    public Guid Id { get; set; }
    public Guid RecipeId { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string GrantedBy { get; set; }  = default!;
    public DateTime Granted { get; set; }
}