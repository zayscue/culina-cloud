using Microsoft.EntityFrameworkCore;
using CulinaCloud.BuildingBlocks.Common.Interfaces;
using CulinaCloud.Analytics.Domain.Entities;

namespace CulinaCloud.Analytics.Application.Interfaces
{
    public interface IApplicationDbContext : IDbContext
    {
        DbSet<RecipePopularity> RecipePopularity { get; set; }
        DbSet<RecipeSimilarity> RecipeSimilarity { get; set; }
    }
}
