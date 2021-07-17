using MediatR;
using CulinaCloud.BuildingBlocks.Common;

namespace CulinaCloud.BuildingBlocks.Application.Common.Models
{
    public class AggregateEventNotification<TAggregateEvent, T> : INotification where TAggregateEvent : IAggregateEvent<T>
    {
        public AggregateEventNotification(TAggregateEvent aggregateEvent)
        {
            AggregateEvent = aggregateEvent;
        }
        
        public TAggregateEvent AggregateEvent { get; }
    }
}