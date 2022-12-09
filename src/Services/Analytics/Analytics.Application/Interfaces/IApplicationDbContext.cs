using Microsoft.EntityFrameworkCore;
using CulinaCloud.BuildingBlocks.Common.Interfaces;
using CulinaCloud.Analytics.Domain.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CulinaCloud.Analytics.Application.Interfaces
{
    public interface IApplicationDbContext : IDbContext
    {
        DatabaseFacade Database { get; }
        DbSet<RecipePopularity> RecipePopularity { get; set; }
        DbSet<RecipeSimilarity> RecipeSimilarity { get; set; }
    }
}
