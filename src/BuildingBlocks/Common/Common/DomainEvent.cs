using System;
using System.Collections.Generic;

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

        public Guid EventId { get; protected set; } = Guid.NewGuid();
        public string EventName => GetType().Name;
        public DateTimeOffset Occurred { get; protected set; } = DateTime.UtcNow;
        public string RaisedBy { get; set; }
        public string Details { get; set; }
        public bool IsPublished { get; set; }
    }
}
