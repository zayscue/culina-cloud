using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using CulinaCloud.EventStore.Domain.Entities;

namespace CulinaCloud.EventStore.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DatabaseFacade Database { get; }

        DbSet<Event> Events { get; set; }
        DbSet<Aggregate> Aggregates { get; set; }
    }
}
