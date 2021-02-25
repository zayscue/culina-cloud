using System;
using System.Text.Json;

namespace CulinaCloud.EventStore.Application.Events.Commands.StoreEvent
{
    public class GenericAggregateEvent
    {
        public Guid EventId { get; set; }
        public string EventName { get; set; }
        public string AggregateType { get; set; }
        public JsonDocument Data { get; set; }
        public DateTimeOffset Occurred { get; set; }
        public string RaisedBy { get; set; }
        public string Details { get; set; }
    }
}
