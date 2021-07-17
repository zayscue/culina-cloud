using CulinaCloud.BuildingBlocks.Common.Interfaces;

namespace CulinaCloud.BuildingBlocks.PostMaster.Abstractions
{
    public interface IEventInboxDbContext : IDbContext, IEventInboxContext
    {
    }
}
