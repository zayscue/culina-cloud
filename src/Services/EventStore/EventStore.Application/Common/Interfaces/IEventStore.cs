using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CulinaCloud.EventStore.Domain.Entities;

namespace CulinaCloud.EventStore.Application.Common.Interfaces
{
    public interface IEventStore
    {
        Task StoreEventsAsync(Guid aggregateId, IEnumerable<Event> events, CancellationToken cancellationToken = default);
        Task<IEnumerable<Event>> LoadEventsAsync(Guid aggregateId, CancellationToken cancellationToken = default);
    }
}
