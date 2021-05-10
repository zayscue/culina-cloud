using System;
using System.Net.Http.Headers;
using Amazon.SecretsManager;
using Culina.CookBook.Application.Common.Interfaces;
using Culina.CookBook.Infrastructure.Authentication;
using Culina.CookBook.Infrastructure.EventStore;
using Culina.CookBook.Infrastructure.Persistence;
using Culina.CookBook.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Culina.CookBook.Infrastructure
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

            if (isDevelopment)
            {
                services.AddTransient<Auth0SecretsProvider, Auth0UserSecretsProvider>(provider =>
                {
                    var config = provider.GetService<IConfiguration>();
                    return new Auth0UserSecretsProvider(config);
                });
            }
            else
            {
                services.AddTransient<Auth0SecretsProvider, Auth0AWSSecretsProvider>(provider =>
                {
                    var secretsManager = new AmazonSecretsManagerClient();
                    return new Auth0AWSSecretsProvider(secretsManager);
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
