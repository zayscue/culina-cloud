namespace CulinaCloud.Web.BFF.APIGateway.Models
{
    public record ApplicationUserDto
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? DisplayName { get; set; }
        public string? Picture { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
