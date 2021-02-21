using System.Reflection;
using Microsoft.EntityFrameworkCore;
using CulinaCloud.EventStore.Application.Common.Interfaces;
using CulinaCloud.EventStore.Domain.Entities;

namespace CulinaCloud.EventStore.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) {}

        public DbSet<Event> Events { get; set; }
        public DbSet<Aggregate> Aggregates { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("EventStore");

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
