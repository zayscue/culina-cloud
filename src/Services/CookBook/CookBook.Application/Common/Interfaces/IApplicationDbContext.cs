using System.Threading;
using System.Threading.Tasks;
using Culina.CookBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Culina.CookBook.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
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

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
