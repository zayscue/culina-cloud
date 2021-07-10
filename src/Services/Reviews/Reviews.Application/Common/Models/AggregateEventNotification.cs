using CulinaCloud.BuildingBlocks.Common;
using MediatR;

namespace CulinaCloud.Reviews.Application.Common.Models
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