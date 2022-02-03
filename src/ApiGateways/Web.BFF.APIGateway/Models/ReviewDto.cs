namespace CulinaCloud.Web.BFF.APIGateway.Models;

public record ReviewDto
{
    public Guid? Id { get; set; }
    public Guid RecipeId { get; set; }
    public string UserId { get; set; }
    public int Rating { get; set; }
    public string? Comments { get; set; }
    public string? CreatedBy { get; set; }
    public string? LastModifiedBy { get; set; }
}