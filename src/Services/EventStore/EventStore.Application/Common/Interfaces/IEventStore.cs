using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CulinaCloud.EventStore.Domain.Entities;

namespace CulinaCloud.EventStore.Application.Common.Interfaces
{
    public interface IEventStore
    {
        Task SaveChangesAsync(IEnumerable<Event> events);
        Task<IEnumerable<Event>> GetEventsFor(Guid aggregateId);
    }
}
