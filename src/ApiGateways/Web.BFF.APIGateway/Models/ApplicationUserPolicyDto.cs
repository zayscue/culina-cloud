namespace CulinaCloud.Web.BFF.APIGateway.Models
{
    public record ApplicationUserPolicyDto
    {
        public Guid RecipeId { get; set; }
        public string? UserId { get; set; }
        public bool IsAFavorite { get; set; }
        public bool IsOwner { get; set; }
        public bool CanEdit { get; set; }
        public bool CanShare { get; set; }
    }
}
