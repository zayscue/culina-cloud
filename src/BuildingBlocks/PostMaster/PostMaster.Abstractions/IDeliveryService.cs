using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.BuildingBlocks.PostMaster.Abstractions
{
    public interface IDeliveryService
    {
        Task<bool> CheckHealth(CancellationToken cancellationToken = default);
        Task DeliverEvents(CancellationToken cancellationToken = default);
    }
}
