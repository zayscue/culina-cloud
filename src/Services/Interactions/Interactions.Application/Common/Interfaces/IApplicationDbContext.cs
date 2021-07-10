using System.Threading;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.Common;
using CulinaCloud.Interactions.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CulinaCloud.Interactions.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DatabaseFacade Database { get; }
        DbSet<Review> Reviews { get; set; }
        DbSet<AggregateEventEntity> EventOutbox { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}