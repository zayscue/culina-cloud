using System;

namespace CulinaCloud.BuildingBlocks.Common
{
    public abstract class AggregateEvent : DomainEvent 
    {
        public abstract string AggregateType { get; }
        public Guid AggregateId { get; set; }
        public object Data { get; set; }
    }
}
