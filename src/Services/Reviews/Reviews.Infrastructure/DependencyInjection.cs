using System;
using System.Net.Http.Headers;
using Amazon.SecretsManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using CulinaCloud.BuildingBlocks.Authentication.Auth0;
using CulinaCloud.BuildingBlocks.Authentication.Auth0.Secrets.Providers;
using CulinaCloud.BuildingBlocks.Authentication.Auth0.Settings;
using CulinaCloud.BuildingBlocks.Common.Interfaces;
using CulinaCloud.Reviews.Application.Common.Interfaces;
using CulinaCloud.Reviews.Infrastructure.Persistence;
using CulinaCloud.Reviews.Infrastructure.Services;

namespace CulinaCloud.Reviews.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration, bool isDevelopment = false)
        {
            var connectionString = configuration["ConnectionString"];

            services.Configure<Auth0Settings>(configuration.GetSection("Auth0"));
            services.Configure<EventStoreSettings>(configuration.GetSection("EventStore"));
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IAggregateEventService, AggregateEventService>();

            if (isDevelopment)
            {
                services.AddTransient<Auth0SecretsProvider, Auth0UserSecretsProvider>(provider =>
                {
                    var config = provider.GetService<IConfiguration>();
                    return new Auth0UserSecretsProvider(config, "ClientId", "ClientSecret");
                });
            }
            else
            {
                services.AddTransient<Auth0SecretsProvider, Auth0AWSSecretsProvider>(provider =>
                {
                    var secretsManager = new AmazonSecretsManagerClient();
                    return new Auth0AWSSecretsProvider(secretsManager, "CulinaCloud/InteractionsAPI/OAuthSecrets");
                });
            }

            services.AddSingleton<ITokenServiceManager, Auth0TokenServiceManager>(provider =>
            {
                var dateTime = provider.GetService<IDateTime>();
                var settings = provider.GetService<IOptions<Auth0Settings>>();
                var secretsProvider = provider.GetService<Auth0SecretsProvider>();
                return new Auth0TokenServiceManager(dateTime, settings, secretsProvider);
            });

            services.AddHttpClient<IEventStoreService, EventStoreService>((client, provider) =>
            {
                var settings = provider.GetService<IOptions<EventStoreSettings>>();
                var baseAddress = new Uri(settings.Value.BaseAddress);
                client.BaseAddress = baseAddress;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var tokenServiceManager = provider.GetService<ITokenServiceManager>();
                var tokenService = tokenServiceManager.GetTokenService(settings.Value.Audience);
                return new EventStoreService(tokenService, client);
            });

            return services;
        }
    }
}