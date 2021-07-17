using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.BuildingBlocks.Common.Interfaces
{
    public interface IAggregateEventService
    {
        Task Publish<T>(IAggregateEvent<T> aggregateEvent, CancellationToken cancellationToken = default);
    }
}