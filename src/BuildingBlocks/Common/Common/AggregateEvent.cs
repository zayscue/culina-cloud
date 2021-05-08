using System;

namespace CulinaCloud.BuildingBlocks.Common
{
    public abstract class AggregateEvent : DomainEvent 
    {
        public abstract string AggregateType { get; }
        protected Guid AggregateId { get; set; }
        protected object Data { get; set; }
    }
}
