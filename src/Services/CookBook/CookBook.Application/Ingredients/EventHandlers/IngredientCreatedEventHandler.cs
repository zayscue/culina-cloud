using System.Threading;
using System.Threading.Tasks;
using Culina.CookBook.Application.Common.Models;
using Culina.CookBook.Domain.Events;
using CulinaCloud.BuildingBlocks.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Culina.CookBook.Application.Ingredients.EventHandlers
{
    public class IngredientCreatedEventHandler : INotificationHandler<AggregateEventNotification<IAggregateEvent<IIngredientCreated>, IIngredientCreated>>
    {
        private readonly ILogger<IngredientCreatedEventHandler> _logger;

        public IngredientCreatedEventHandler(ILogger<IngredientCreatedEventHandler> logger)
        {
            _logger = logger;
        }
        
        public Task Handle(AggregateEventNotification<IAggregateEvent<IIngredientCreated>, IIngredientCreated> notification, CancellationToken cancellationToken)
        {
            var aggregateEvent = notification.AggregateEvent;
            
            _logger.LogInformation("CookBook.API Aggregate Event: {AggregateEvent}", aggregateEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
}