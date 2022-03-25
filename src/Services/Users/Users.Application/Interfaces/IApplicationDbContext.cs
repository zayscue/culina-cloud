using CulinaCloud.BuildingBlocks.Common.Interfaces;
using CulinaCloud.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.Users.Application.Interfaces
{
    public interface IApplicationDbContext : IDbContext
    {
        DbSet<Favorite> Favorites { get; set; }
        DbSet<RecipeEntitlement> RecipeEntitlements { get; set; }
        DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
