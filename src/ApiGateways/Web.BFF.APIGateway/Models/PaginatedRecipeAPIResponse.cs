namespace CulinaCloud.Web.BFF.APIGateway.Models
{
    public record PaginatedRecipeAPIResponse
    {
        public List<RecipeAPIResponse>? Items { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }
}
