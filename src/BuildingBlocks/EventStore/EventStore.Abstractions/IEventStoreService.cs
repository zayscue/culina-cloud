using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.Common;

namespace CulinaCloud.BuildingBlocks.EventStore.Abstractions
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
