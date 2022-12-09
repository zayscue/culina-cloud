namespace CulinaCloud.Web.BFF.APIGateway.Models
{
    public record IngredientDto
    {
        public Guid Id { get; set; }
        public string IngredientName { get; set; } = default!;
    }
}
