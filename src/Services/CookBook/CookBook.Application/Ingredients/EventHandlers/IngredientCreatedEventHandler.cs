using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Culina.CookBook.Application.Common.Interfaces;
using Culina.CookBook.Application.Common.Models;
using Culina.CookBook.Domain.Events;
using CulinaCloud.BuildingBlocks.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Culina.CookBook.Application.Ingredients.EventHandlers
{
    public class IngredientCreatedEventHandler : INotificationHandler<AggregateEventNotification<IAggregateEvent<IIngredientCreated>, IIngredientCreated>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<IngredientCreatedEventHandler> _logger;

        public IngredientCreatedEventHandler(IApplicationDbContext context, ILogger<IngredientCreatedEventHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Handle(AggregateEventNotification<IAggregateEvent<IIngredientCreated>, IIngredientCreated> notification, CancellationToken cancellationToken)
        {
            var aggregateEvent = notification.AggregateEvent;

            _logger.LogInformation("CookBook.API Aggregate Event: {AggregateEvent}", aggregateEvent.GetType().Name);

            var aggregateEventEntity = await _context.EventOutbox
                .Where(x => x.EventId == aggregateEvent.EventId)
                .SingleOrDefaultAsync(cancellationToken);

            aggregateEventEntity.IsPublished = true;

            _context.EventOutbox.Update(aggregateEventEntity);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}