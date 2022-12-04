namespace CulinaCloud.Web.BFF.APIGateway.Models
{
    public record TagDto
    {
        public Guid Id { get; set; }
        public string TagName { get; set; } = default!;
    }
}
