using System;

namespace CulinaCloud.BuildingBlocks.Common
{
    public abstract class AggregateEvent<T> : DomainEvent 
    {
        public abstract string AggregateType { get; }
        public Guid AggregateId { get; set; }
        public T Data { get; set; }
    }
}
