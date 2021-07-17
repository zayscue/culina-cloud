using CulinaCloud.BuildingBlocks.Common;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.BuildingBlocks.PostMaster.Abstractions
{
    public interface IEventInboxContext
    {
        public DbSet<AggregateEventEntity> EventInbox { get; set; }
    }
}
