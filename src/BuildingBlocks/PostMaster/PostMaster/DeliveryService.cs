using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CulinaCloud.BuildingBlocks.EventStore.Abstractions;
using CulinaCloud.BuildingBlocks.PostMaster.Abstractions;

namespace CulinaCloud.BuildingBlocks.PostMaster
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IEventStoreService _eventStore;
        private readonly IEventOutboxDbContext _context;
        private ILogger<DeliveryService> _logger;

        public DeliveryService(
            IEventStoreService eventStore,
            IEventOutboxDbContext context,
            ILogger<DeliveryService> logger)
        {
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> CheckHealth(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _eventStore.CheckHealth(cancellationToken);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task DeliverEvents(CancellationToken cancellationToken = default)
        {
            try
            {
                var queryResult = await _context.EventOutbox
                    .AsNoTracking()
                    .Where(x => x.IsStored == false)
                    .ToListAsync(cancellationToken);
                var unstoredAggregateEventGroups = queryResult.GroupBy(x => x.AggregateId);
                foreach (var unstoredAggregateEventGroup in unstoredAggregateEventGroups)
                {
                    var aggregateId = unstoredAggregateEventGroup.Key;
                    var aggregateEventEntities = unstoredAggregateEventGroup
                        .Select(x => x)
                        .OrderBy(x => x.Occurred)
                        .ToList();
                    var aggregateEvents = aggregateEventEntities.Select(x => x.ToAggregateEvent());
                    await _eventStore.StoreEventsAsync(aggregateId, aggregateEvents, cancellationToken);
                    foreach (var aggregateEventEntity in aggregateEventEntities)
                    {
                        aggregateEventEntity.IsStored = true;
                        _context.EventOutbox.Update(aggregateEventEntity);
                    }
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while delivering events to the EventStoreService", e);
            }
        }
    }
}
