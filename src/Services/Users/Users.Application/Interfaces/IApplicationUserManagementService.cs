using CulinaCloud.Users.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.Users.Application.Interfaces
{
    public interface IApplicationUserManagementService
    {
        Task<ApplicationUser> GetApplicationUser(string userId, CancellationToken cancellation = default);
        Task<ApplicationUser> SaveApplicationUser(ApplicationUser applicationUser, CancellationToken cancellation = default);
    }
}
