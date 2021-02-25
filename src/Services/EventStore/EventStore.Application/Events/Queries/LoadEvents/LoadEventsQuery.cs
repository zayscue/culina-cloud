using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MediatR;
using AutoMapper;
using CulinaCloud.EventStore.Application.Common.Interfaces;

namespace CulinaCloud.EventStore.Application.Events.Queries.LoadEvents
{
    public class LoadEventsQuery : IRequest<IEnumerable<EventDto>>
    {
        public Guid AggregateId { get; set; }
    }

    public class LoadEventsQueryHandler : IRequestHandler<LoadEventsQuery, IEnumerable<EventDto>>
    {
        private readonly IEventStore _eventStore;
        private readonly IMapper _mapper;

        public LoadEventsQueryHandler(IEventStore eventStore, IMapper mapper)
        {
            _eventStore = eventStore;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventDto>> Handle(LoadEventsQuery request, CancellationToken cancellationToken)
        {
            var aggregateId = request.AggregateId;

            var events = await _eventStore.LoadEventsAsync(aggregateId, cancellationToken);

            var dtos = events.Select(x => _mapper.Map<EventDto>(x));

            return dtos;
        }
    }
}
