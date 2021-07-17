using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CulinaCloud.Analytics.Application.Interfaces;
using CulinaCloud.Analytics.Domain.Entities;
using CulinaCloud.BuildingBlocks.Common;
using CulinaCloud.BuildingBlocks.Common.Interfaces;
using CulinaCloud.BuildingBlocks.CurrentUser.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CulinaCloud.Analytics.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public ApplicationDbContext(
            DbContextOptions options,
            ILoggerFactory loggerFactory,
            ICurrentUserService currentUserService,
            IDateTime dateTime) : base(options)
        {
            _loggerFactory = loggerFactory;
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }

        public DbSet<RecipePopularity> RecipePopularity { get; set; }
        public DbSet<RecipeSimilarity> RecipeSimilarity { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy ??= _currentUserService.UserId;
                        if (entry.Entity.Created == default)
                        {
                            entry.Entity.Created = _dateTime.Now;
                        }
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy ??= _currentUserService.UserId;
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
            builder.HasDefaultSchema("Analytics");

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
