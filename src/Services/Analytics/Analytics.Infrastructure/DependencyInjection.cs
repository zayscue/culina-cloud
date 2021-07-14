using CulinaCloud.Analytics.Application.Interfaces;
using CulinaCloud.Analytics.Infrastructure.Persistence;
using CulinaCloud.Analytics.Infrastructure.Services;
using CulinaCloud.BuildingBlocks.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CulinaCloud.Analytics.Infrastructure
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


            return services;
        }
    }
}
