using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CulinaCloud.Users.Domain.Entities
{
    public record Statistics
    {
        public long TotalActiveApplicationUsers { get; set; }
        public ICollection<DailyApplicationUsersStatistics> DailyApplicationUsersStatistics { get; set; } = new 
            List<DailyApplicationUsersStatistics>();
    }

    public record DailyApplicationUsersStatistics
    {
        public DateTime Date { get; set; }
        public int Logins { get; set; }
        public int SignUps { get; set; }
    }
}
