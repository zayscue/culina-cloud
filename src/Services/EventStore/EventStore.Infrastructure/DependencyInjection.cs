using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using CulinaCloud.EventStore.Application.Common.Interfaces;
using CulinaCloud.EventStore.Infrastructure.Persistence;
using CulinaCloud.EventStore.Infrastructure.Services;

namespace CulinaCloud.EventStore.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["ConnectionString"];

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            
            services.AddScoped<IEventStore, DbEventStore>(provider =>
            {
                var context = provider.GetService<ApplicationDbContext>();
                return new DbEventStore(context);
            });

            return services;
        }
    }
}
