﻿using System;
using System.Text.Json;

namespace CulinaCloud.EventStore.Domain.Entities
{
    public class Event
    {
        public Guid EventId { get; set; }
        public string EventName { get; set; }
        public Guid AggregateId { get; set; }
        public int Version { get; set; }
        public JsonDocument Data { get; set; }
        public DateTimeOffset Occurred { get; set; }
        public string RaisedBy { get; set; }
        public string Details { get; set; }

        public Aggregate Aggregate { get; set; }
    }
}
