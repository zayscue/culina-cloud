using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Culina.CookBook.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Culina.CookBook.API.BackgroundServices
{
    public class EventDeliveryBackgroundServiceSettings
    {
        public int PollingIntervalMS { get; set; }
    }

    public class EventDeliveryService : IEventDeliveryService
    {
        private readonly IEventStoreService _eventStore;
        private readonly IApplicationDbContext _context;
        private ILogger<EventDeliveryService> _logger;

        public EventDeliveryService(
            IEventStoreService eventStore,
            IApplicationDbContext context,
            ILogger<EventDeliveryService> logger)
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
                    var aggregateEventEntities = unstoredAggregateEventGroup.Select(x => x).ToList();
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

    public class EventDeliveryBackgroundService : BackgroundService
    {
        private readonly EventDeliveryBackgroundServiceSettings _settings;
        private readonly ILogger<EventDeliveryBackgroundService> _logger;
        private bool _isHealthy;

        public IServiceProvider Services { get; }

        public EventDeliveryBackgroundService(
            IServiceProvider services,
            IOptions<EventDeliveryBackgroundServiceSettings> settings,
            ILogger<EventDeliveryBackgroundService> logger)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _isHealthy = true;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("EventDeliveryService is starting.");

            stoppingToken.Register(() => _logger.LogDebug("EventDeliveryService background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = Services.CreateScope())
                {
                    var eventDeliveryService =
                        scope.ServiceProvider
                            .GetRequiredService<IEventDeliveryService>();

                    var isHealthy = await eventDeliveryService.CheckHealth(stoppingToken);
                    if (isHealthy)
                    {
                        _logger.LogDebug("EventDeliveryService background task is doing background work.");
                        await eventDeliveryService.DeliverEvents(stoppingToken);
                    }
                    else
                    {
                        if (_isHealthy != isHealthy)
                        {
                            _logger.LogError("EventDeliveryService did no work because the EventStoreService is not healthy");
                        }
                    }
                    _isHealthy = isHealthy;
                }
                await Task.Delay(_settings.PollingIntervalMS, stoppingToken);
            }

            _logger.LogDebug("EventDeliveryService background task is stopping.");
        }
    }
}