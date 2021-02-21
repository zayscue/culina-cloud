using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;

namespace CulinaCloud.BuildingBlocks.Common
{
    public interface IHasDomainEvent
    {
        public List<DomainEvent> DomainEvents { get; set; }
    }

    public abstract class DomainEvent
    {
        protected DomainEvent()
        {
            EventId = Guid.NewGuid();
            Occurred = DateTimeOffset.UtcNow;
        }

        protected virtual string[] ReservedProperties
        {
            get
            {
                return new string[]
                {
                    "EventId",
                    "Occurred",
                    "RaisedBy",
                    "Details",
                    "IsPublished"
                };
            }
        }

        public Guid EventId { get; protected set; } = Guid.NewGuid();
        public DateTimeOffset Occurred { get; protected set; } = DateTime.UtcNow;
        public string RaisedBy { get; set; }
        public string Details { get; set; }
        public bool IsPublished { get; set; }

        public virtual IDictionary<string, object> Data()
        {
            var dataDict = new Dictionary<string, object>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(this.GetType()))
            {
                if (!ReservedProperties.Contains(property.Name))
                {
                    dataDict.Add(property.Name, property.GetValue(this));
                }
            }
            return dataDict;
        }

        public virtual string Serialize()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            var jsonString = JsonSerializer.Serialize(new
            {
                eventId = EventId,
                occurred = Occurred,
                raisedBy = RaisedBy,
                details = Details,
                data = Data()
            });
            return jsonString;
        }
    }

    public abstract class AggregateDomainEvent : DomainEvent
    {
        protected AggregateDomainEvent()
        {
            AggregateId = Guid.NewGuid();
        }

        protected AggregateDomainEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }

        protected override string[] ReservedProperties
        {
            get
            {
                return new string[]
                {
                    "EventId",
                    "Occurred",
                    "RaisedBy",
                    "Details",
                    "IsPublished",
                    "AggregateType",
                    "AggregateId"
                };
            }
        }

        public abstract string AggregateType { get; }
        public Guid AggregateId { get; private set; }

        public override string Serialize()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            var jsonString = JsonSerializer.Serialize(new
            {
                eventId = EventId,
                aggregateId = AggregateId,
                aggregateType = AggregateType,
                occurred = Occurred,
                raisedBy = RaisedBy,
                details = Details,
                data = Data()
            });
            return jsonString;
        }
    }
}
