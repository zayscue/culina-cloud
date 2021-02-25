using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CulinaCloud.EventStore.Application.Common.Interfaces;
using CulinaCloud.EventStore.Domain.Entities;

namespace CulinaCloud.EventStore.Application.Events.Commands.StoreEvent
{
    public class StoreEventCommand : IRequest
    {
        public Guid AggregateId { get; set; }
        public List<GenericAggregateEvent> Events { get; set; }
    }

    public class StoreEventCommandHandler : IRequestHandler<StoreEventCommand, Unit>
    {
        private readonly IEventStore _eventStore;

        public StoreEventCommandHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<Unit> Handle(StoreEventCommand request, CancellationToken cancellationToken)
        {
            var aggregateId = request.AggregateId;
            var genericAggregateEvents = request.Events;

            var events = new List<Event>();
            foreach (var genericAggregateEvent in genericAggregateEvents)
            {
                events.Add(new Event
                {
                    EventId = genericAggregateEvent.EventId,
                    EventName = genericAggregateEvent.EventName,
                    Data = genericAggregateEvent.Data,
                    Occurred = genericAggregateEvent.Occurred,
                    RaisedBy = genericAggregateEvent.RaisedBy,
                    Details = genericAggregateEvent.Details,
                    AggregateId = aggregateId,
                    Aggregate = new Aggregate
                    {
                        AggregateId = aggregateId,
                        AggregateType = genericAggregateEvent.AggregateType
                    }
                });
            }
            await _eventStore.StoreEventsAsync(aggregateId, events, cancellationToken);

            return Unit.Value;
        }
    }
}
