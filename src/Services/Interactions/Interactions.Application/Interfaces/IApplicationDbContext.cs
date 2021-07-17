using CulinaCloud.BuildingBlocks.Common.Interfaces;
using CulinaCloud.BuildingBlocks.PostMaster.Abstractions;
using CulinaCloud.Interactions.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CulinaCloud.Interactions.Application.Interfaces
{
    public interface IApplicationDbContext : IDbContext, IEventOutboxDbContext
    {
        DatabaseFacade Database { get; }
        DbSet<Review> Reviews { get; set; }
    }
}
