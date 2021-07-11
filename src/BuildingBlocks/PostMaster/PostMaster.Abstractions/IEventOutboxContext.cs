using CulinaCloud.BuildingBlocks.Common;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.BuildingBlocks.PostMaster.Abstractions
{
    public interface IEventOutboxContext
    {
        public DbSet<AggregateEventEntity> EventOutbox { get; set; }
    }
}
