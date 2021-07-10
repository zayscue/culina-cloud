using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.BuildingBlocks.Common.Interfaces
{
    public interface IEventStoreService
    {
        Task<bool> CheckHealth(CancellationToken cancellationToken = default);

        Task StoreEventsAsync(Guid aggregateId, IEnumerable<AggregateEvent> events,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<AggregateEvent>> LoadEventsAsync(Guid aggregateId,
            CancellationToken cancellationToken = default);
    }
}