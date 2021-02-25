using System;

namespace CulinaCloud.EventStore.Application.Events.Commands.StoreEvent
{
    public class StoreEventResponse
    {
        public Guid AggregateId { get; set; }
    }
}
