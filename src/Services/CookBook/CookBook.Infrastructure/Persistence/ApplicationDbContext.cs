using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Culina.CookBook.Application.Common.Interfaces;
using Culina.CookBook.Domain.Entities;

namespace Culina.CookBook.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeImage> RecipeImages { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<RecipeMetadata> RecipeMetadata { get; set; }
        public DbSet<RecipeNutrition> RecipeNutrition { get; set; }
        public DbSet<RecipeStep> RecipeSteps { get; set; }
        public DbSet<RecipeTag> RecipeTags { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("CookBook");

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
