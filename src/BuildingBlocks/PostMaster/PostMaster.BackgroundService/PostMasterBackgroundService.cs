using System;
using System.Threading;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.PostMaster.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CulinaCloud.BuildingBlocks.PostMaster.BackgroundService
{
    public class PostMasterBackgroundService : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly PostMasterBackgroundServiceSettings _settings;
        private readonly ILogger<PostMasterBackgroundService> _logger;
        private bool _isHealthy;

        public IServiceProvider Services { get; }

        public PostMasterBackgroundService(
            IServiceProvider services,
            IOptions<PostMasterBackgroundServiceSettings> settings,
            ILogger<PostMasterBackgroundService> logger)
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
                            .GetRequiredService<IDeliveryService>();

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
                await Task.Delay(_settings.PollingIntervalMs, stoppingToken);
            }

            _logger.LogDebug("EventDeliveryService background task is stopping.");
        }
    }
}
