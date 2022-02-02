namespace CulinaCloud.Web.BFF.APIGateway.Models;

public record RecipeFeedItemDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int EstimatedMinutes { get; set; }
    public string? Serves { get; set;  }
    public string? Yield { get; set; }
    public string? UserId { get; set; }
    public bool IsAFavorite { get; set; }
    public decimal PopularityScore { get; set; }
    public float? PredictedScore { get; set;  }
}