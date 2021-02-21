using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CulinaCloud.EventStore.Domain.Entities
{
    public class Event
    {
        public Guid EventId { get; set; }
        public Guid? AggregateId { get; set; }
        public int Version { get; set; }
        public JsonDocument Data { get; set; }
        public DateTimeOffset Occurred { get; set; }
        public string RaisedBy { get; set; }
        public string Details { get; set; }
    }
}
