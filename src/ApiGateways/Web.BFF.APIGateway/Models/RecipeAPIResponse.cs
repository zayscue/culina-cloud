namespace CulinaCloud.Web.BFF.APIGateway.Models
{
    public record RecipeAPIResponse
    {
        public RecipePolicy? Policy { get; init; }
        public RecipePopularityDto? Popularity { get; init; }
        public object? Data { get; init; }
    }

    public record RecipePolicy
    {
        public bool IsAFavorite { get; init; }
        public bool CanEdit { get; init; }
        public bool IsOwner { get; init; }
        public bool CanShare { get; init; }
    }
}
