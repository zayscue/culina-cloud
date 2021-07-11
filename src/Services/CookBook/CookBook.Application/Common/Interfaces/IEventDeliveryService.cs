using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.CookBook.Application.Common.Interfaces
{
    public interface IEventDeliveryService
    {
        Task<bool> CheckHealth(CancellationToken cancellationToken = default);
        Task DeliverEvents(CancellationToken cancellationToken = default);
    }
}