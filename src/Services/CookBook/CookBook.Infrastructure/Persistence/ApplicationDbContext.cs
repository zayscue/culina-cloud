using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.Common;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using CulinaCloud.CookBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CulinaCloud.CookBook.Infrastructure.Persistence
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
            optionsBuilder
                .UseLoggerFactory(_loggerFactory);
        }


        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeImage> RecipeImages { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<RecipeMetadata> RecipeMetadata { get; set; }
        public DbSet<RecipeNutrition> RecipeNutrition { get; set; }
        public DbSet<RecipeStep> RecipeSteps { get; set; }
        public DbSet<RecipeTag> RecipeTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<AggregateEventEntity> EventOutbox { get; set; }

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
                        entry.Entity.LastModified ??= _dateTime.Now;
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
            builder.HasDefaultSchema("CookBook");

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
