using Amazon.SecretsManager;
using Culina.CookBook.Application.Common.Interfaces;
using Culina.CookBook.Infrastructure.EventStore;
using Culina.CookBook.Infrastructure.Persistence;
using Culina.CookBook.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Culina.CookBook.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration, bool isDevelopment = false)
        {
            var connectionString = configuration["ConnectionString"];

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddTransient<IDateTime, DateTimeService>();

            if (isDevelopment)
            {
                services.AddTransient<EventStoreSecretsProvider, EventStoreUserSecretsProvider>(provider =>
                {
                    var config = provider.GetService<IConfiguration>();
                    return new EventStoreUserSecretsProvider(config);
                });
            }
            else
            {
                services.AddTransient<EventStoreSecretsProvider, EventStoreAWSSecretsProvider>(provider =>
                {
                    var secretsManager = new AmazonSecretsManagerClient();
                    return new EventStoreAWSSecretsProvider(secretsManager);
                });
            }
            
            return services;
        }
    }
}
