using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.Common;
using CulinaCloud.BuildingBlocks.Common.Interfaces;
using CulinaCloud.BuildingBlocks.CurrentUser.Abstractions;
using CulinaCloud.BuildingBlocks.PostMaster.Persistence;
using CulinaCloud.Interactions.Application.Interfaces;
using CulinaCloud.Interactions.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CulinaCloud.Interactions.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IDateTime _dateTime;

        public ApplicationDbContext(
            DbContextOptions options,
            ILoggerFactory loggerFactory,
            IDateTime dateTime) : base(options)
        {
            _loggerFactory = loggerFactory;
            _dateTime = dateTime;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }

        public DbSet<Review> Reviews { get; set; }
        public DbSet<AggregateEventEntity> EventOutbox { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entry.Entity.Created == default)
                        {
                            entry.Entity.Created = _dateTime.Now;
                        }
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    case EntityState.Deleted:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("Interactions");

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.ApplyConfiguration(new EventOutboxConfiguration());

            base.OnModelCreating(builder);
        }
    }
}