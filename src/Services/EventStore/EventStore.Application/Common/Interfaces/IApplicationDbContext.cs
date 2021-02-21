using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CulinaCloud.EventStore.Domain.Entities;

namespace CulinaCloud.EventStore.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Event> Events { get; set; }
        DbSet<Aggregate> Aggregates { get; set; }
    }
}
