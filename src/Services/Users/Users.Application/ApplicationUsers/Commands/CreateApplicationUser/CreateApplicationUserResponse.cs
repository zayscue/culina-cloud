using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.Users.Domain.Entities;

namespace CulinaCloud.Users.Application.ApplicationUsers.Commands.CreateApplicationUser
{
    public class CreateApplicationUserResponse : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Picture { get; set; }
    }
}