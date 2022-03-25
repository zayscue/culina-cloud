using CulinaCloud.BuildingBlocks.Common;

namespace CulinaCloud.Users.Domain.Entities
{
    public class ApplicationUser : AuditableEntity
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Picture { get; set; }
    }
}