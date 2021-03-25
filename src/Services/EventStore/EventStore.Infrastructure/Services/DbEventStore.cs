using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CulinaCloud.EventStore.Domain.Entities;
using CulinaCloud.EventStore.Application.Common.Interfaces;

namespace CulinaCloud.EventStore.Infrastructure.Services
{
    public class DbEventStore : IEventStore
    {
        private readonly IApplicationDbContext _context;
        
        public DbEventStore(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> LoadEventsAsync(Guid aggregateId, CancellationToken cancellationToken = default)
        {
            var events = await _context.Events
                .AsNoTracking()
                .Include(x => x.Aggregate)
                .Where(x => x.AggregateId == aggregateId)
                .ToListAsync(cancellationToken);
            return events;
        }

        public async Task StoreEventsAsync(Guid aggregateId, IEnumerable<Event> events, CancellationToken cancellationToken = default)
        {
            var trn = _context.Database.CurrentTransaction ?? (await _context.Database.BeginTransactionAsync(cancellationToken));
            try
            {
                foreach (var @event in events)
                {
                    var eventId = @event.EventId;
                    var eventName = @event.EventName;
                    var data = @event.Data;
                    var occurred = @event.Occurred;
                    var raisedBy = !string.IsNullOrWhiteSpace(@event.RaisedBy) ? @event.RaisedBy : "";
                    var details = !string.IsNullOrWhiteSpace(@event.Details) ? @event.Details : "";
                    var aggregateType = @event.Aggregate.AggregateType;
                    await _context.Database.ExecuteSqlInterpolatedAsync(@$"
                    call ""EventStore"".""StoreEvent""(
                      {eventId},
                      {eventName},
                      {data},
                      {occurred},
                      {raisedBy},
                      {details},
                      {aggregateId},
                      {aggregateType}
                    );
                ", cancellationToken);
                }
                await trn.CommitAsync(cancellationToken);
            } 
            catch (Exception)
            {
                await trn.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
