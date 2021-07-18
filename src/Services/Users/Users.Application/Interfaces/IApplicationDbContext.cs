using CulinaCloud.BuildingBlocks.Common.Interfaces;
using CulinaCloud.BuildingBlocks.PostMaster.Abstractions;
using CulinaCloud.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CulinaCloud.Users.Application.Interfaces
{
    public interface IApplicationDbContext : IDbContext, IEventOutboxDbContext
    {
        DatabaseFacade Database { get; }
        DbSet<Favorite> Favorites { get; set; }
    }
}
