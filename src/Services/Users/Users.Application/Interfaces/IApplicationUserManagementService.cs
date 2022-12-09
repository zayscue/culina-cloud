using System;
using CulinaCloud.Users.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.Users.Application.Interfaces
{
    public interface IApplicationUserManagementService
    {
        Task<ApplicationUser> GetApplicationUser(string userId, CancellationToken cancellation = default);
        Task<ApplicationUser> SaveApplicationUser(ApplicationUser applicationUser, CancellationToken cancellation = default);
        Task<Domain.Entities.Statistics> GetApplicationUsersStatistics(DateTime from , DateTime to, CancellationToken cancellation = default);
    }
}
