using System;
using System.Text.Json;

namespace Culina.CookBook.Application.Common.Models
{
    public class GenericAggregateEvent
    {
        
        public Guid EventId { get; set; }
        public string EventName { get; set; }
        public Guid AggregateId { get; set; }
        public string AggregateType { get; set; }
        public int Version { get; set; }
        public JsonDocument Data { get; set; }
        public DateTimeOffset Occurred { get; set; }
        public string RaisedBy { get; set; }
        public string Details { get; set; }
    }
}