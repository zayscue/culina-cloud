using System.Threading;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.Common;
using CulinaCloud.BuildingBlocks.Common.Interfaces;
using CulinaCloud.Interactions.Application.Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CulinaCloud.Interactions.Infrastructure.Services
{
    public class AggregateEventService : IAggregateEventService
    {
        private readonly ILogger<AggregateEventService> _logger;
        private readonly IPublisher _mediator;

        public AggregateEventService(ILogger<AggregateEventService> logger, IPublisher mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        public async Task Publish<T>(IAggregateEvent<T> aggregateEvent, CancellationToken cancellationToken = default)
        {
            var notification = new AggregateEventNotification<IAggregateEvent<T>, T>(aggregateEvent);
            _logger.LogInformation("Publishing aggregate event. Event - {event}", aggregateEvent.GetType().Name);
            await _mediator.Publish(notification, cancellationToken);
        }
    }
}