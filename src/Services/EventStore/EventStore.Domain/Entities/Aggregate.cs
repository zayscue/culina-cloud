using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CulinaCloud.EventStore.Domain.Entities
{
    public class Aggregate
    {
        public Guid AggregateId { get; set; }
        public string AggregateType { get; set; }
        public int Version { get; set; }

        public IList<Event> Events { get; private set; } = new List<Event>();
    }
}
