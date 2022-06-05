using Amazon.SecretsManager;
using CulinaCloud.BuildingBlocks.Authentication.Abstractions;
using CulinaCloud.BuildingBlocks.Authentication.Auth0;
using CulinaCloud.BuildingBlocks.Authentication.Auth0.Secrets.Providers;
using CulinaCloud.BuildingBlocks.Authentication.Auth0.Settings;
using CulinaCloud.BuildingBlocks.Common.Interfaces;
using CulinaCloud.Users.Application.Interfaces;
using CulinaCloud.Users.Infrastructure.Persistence;
using CulinaCloud.Users.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CulinaCloud.Users.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration, bool isDevelopment = false)
        {
            var connectionString = configuration["ConnectionString"];

            services.Configure<Auth0Settings>(configuration.GetSection("Auth0"));
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddTransient<IDateTime, DateTimeService>();

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
                    return new Auth0AWSSecretsProvider(secretsManager, "CulinaCloud/UsersAPI/OAuthSecrets");
                });
            }

            services.AddSingleton<ITokenServiceManager, Auth0TokenServiceManager>(provider =>
            {
                var dateTime = provider.GetService<IDateTime>();
                var settings = provider.GetService<IOptions<Auth0Settings>>();
                var secretsProvider = provider.GetService<Auth0SecretsProvider>();
                return new Auth0TokenServiceManager(dateTime, settings, secretsProvider);
            });

            return services;
        }
    }
}
