using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.BuildingBlocks.Common.Interfaces
{
    public interface IDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
