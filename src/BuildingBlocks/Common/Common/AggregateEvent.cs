using System;
using System.Text.Json;

namespace CulinaCloud.BuildingBlocks.Common
{
    public interface IAggregateEvent<out T>
    {
        Guid EventId { get; }
        string EventName { get; }
        DateTimeOffset Occurred { get; }
        string  AggregateType { get; }
        Guid AggregateId { get; }
        string RaisedBy { get; }
        string  Details { get; }
        T Data { get; }
    }

    public interface IAggregateEventConvertible
    {
        AggregateEvent ToAggregateEvent();
    }

    public class AggregateEvent : IAggregateEvent<JsonDocument>
    {
        public Guid EventId { get; set; }
        public string EventName { get; set; }
        public DateTimeOffset Occurred { get; set; }
        public string AggregateType { get; set; }
        public Guid AggregateId { get; set; }
        public string RaisedBy { get; set; }
        public string Details { get; set; }
        public JsonDocument Data { get; set; }
    }

    public class AggregateEventEntity : AggregateEvent, IAggregateEventConvertible
    {
        public bool IsStored { get; set; }
        public bool IsPublished { get; set; }
        
        public AggregateEventEntity() {}

        public AggregateEventEntity(AggregateEvent @aggregateEvent)
        {
            EventId = @aggregateEvent.EventId;
            EventName = @aggregateEvent.EventName;
            Occurred = @aggregateEvent.Occurred;
            AggregateType = @aggregateEvent.AggregateType;
            AggregateId = @aggregateEvent.AggregateId;
            RaisedBy = @aggregateEvent.RaisedBy;
            Details = @aggregateEvent.Details;
            Data = @aggregateEvent.Data;
            IsStored = false;
            IsPublished = false;
        }

        public AggregateEvent ToAggregateEvent()
        {
            return new()
            {
                EventId = EventId,
                EventName = EventName,
                Occurred = Occurred,
                AggregateType = AggregateType,
                AggregateId = AggregateId,
                RaisedBy = RaisedBy,
                Details = Details,
                Data = Data
            };
        }
    }
    
    public abstract class AggregateEventBase<T> : IAggregateEvent<T>, IAggregateEventConvertible
    {
        protected AggregateEventBase()
        {
            EventId = Guid.NewGuid();
            Occurred = DateTimeOffset.UtcNow;
        }

        public Guid EventId { get; set; } = Guid.NewGuid();
        public string EventName => GetType().Name;
        public DateTimeOffset Occurred { get; set; } = DateTime.UtcNow;
        public string RaisedBy { get; set; }
        public string Details { get; set; }
        public abstract string AggregateType { get; }
        public Guid AggregateId { get; set; }
        public T Data { get; set; }

        public AggregateEvent ToAggregateEvent()
        {
            return new()
            {
                EventId = EventId,
                EventName =  EventName,
                AggregateId = AggregateId,
                AggregateType = AggregateType,
                Occurred = Occurred,
                RaisedBy = RaisedBy,
                Details = Details,
                Data = JsonDocument.Parse(JsonSerializer.Serialize(Data))
            };
        }
    }
}
