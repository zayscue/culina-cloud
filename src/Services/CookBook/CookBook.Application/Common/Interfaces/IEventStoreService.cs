using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Culina.CookBook.Application.Common.Models;
using CulinaCloud.BuildingBlocks.Common;

namespace Culina.CookBook.Application.Common.Interfaces
{
    public interface IEventStoreService
    {
        Task StoreEventsAsync(Guid aggregateId, IEnumerable<AggregateEvent> events, CancellationToken cancellationToken = default);
        Task<IEnumerable<GenericAggregateEvent>> LoadEventsAsync(Guid aggregateId, CancellationToken cancellationToken = default);
    }
}