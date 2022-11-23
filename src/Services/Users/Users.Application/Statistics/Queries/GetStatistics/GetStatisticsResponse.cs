using System.Collections.Generic;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.Users.Domain.Entities;

namespace CulinaCloud.Users.Application.Statistics.Queries.GetStatistics
{
    public class GetStatisticsResponse : IMapFrom<Domain.Entities.Statistics>
    {
        public long TotalActiveApplicationUsers { get; set; }
        public ICollection<DailyApplicationUsersStatistics> DailyApplicationUsersStatistics { get; set; } = new
            List<DailyApplicationUsersStatistics>();
    }
}
