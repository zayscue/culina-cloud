using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.Common;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using CulinaCloud.CookBook.Application.Common.Models;
using CulinaCloud.CookBook.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CulinaCloud.CookBook.Application.Ingredients.EventHandlers
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