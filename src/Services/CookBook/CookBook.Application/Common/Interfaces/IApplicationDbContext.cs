using System.Threading;
using System.Threading.Tasks;
using Culina.CookBook.Domain.Entities;
using CulinaCloud.BuildingBlocks.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Culina.CookBook.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DatabaseFacade Database { get; }
        DbSet<Ingredient> Ingredients { get; set; }
        DbSet<Recipe> Recipes { get; set; }
        DbSet<RecipeImage> RecipeImages { get; set; }
        DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        DbSet<RecipeMetadata> RecipeMetadata { get; set; }
        DbSet<RecipeNutrition> RecipeNutrition { get; set; }
        DbSet<RecipeStep> RecipeSteps { get; set; }
        DbSet<RecipeTag> RecipeTags { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<Image> Images { get; set; }
        DbSet<AggregateEventEntity> EventOutbox { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
