using System.Threading;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.Common;

namespace CulinaCloud.CookBook.Application.Common.Interfaces
{
    public interface IAggregateEventService
    {
        Task Publish<T>(IAggregateEvent<T> aggregateEvent, CancellationToken cancellationToken = default);
    }
}