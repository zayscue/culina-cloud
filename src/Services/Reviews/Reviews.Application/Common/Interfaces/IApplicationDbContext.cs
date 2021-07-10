using System.Threading;
using System.Threading.Tasks;
using CulinaCloud.Reviews.Domain.Entities;
using CulinaCloud.BuildingBlocks.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CulinaCloud.Reviews.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DatabaseFacade Database { get; }
        DbSet<Review> Reviews { get; set; }
        DbSet<AggregateEventEntity> EventOutbox { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}