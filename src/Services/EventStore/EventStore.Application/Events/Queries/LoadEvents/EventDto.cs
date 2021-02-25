using System;
using System.Text.Json;
using AutoMapper;
using CulinaCloud.EventStore.Application.Common.Mapping;
using CulinaCloud.EventStore.Domain.Entities;

namespace CulinaCloud.EventStore.Application.Events.Queries.LoadEvents
{
    public class EventDto : IMapFrom<Event>
    {
        public Guid EventId { get; set; }
        public string EventName { get; set; }
        public Guid AggregateId { get; set; }
        public string AggregateType { get; set; }
        public int Version { get; set; }
        public JsonDocument Data { get; set; }
        public DateTimeOffset Occurred { get; set; }
        public string RaisedBy { get; set; }
        public string Details { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Event, EventDto>()
                .ForMember(d => d.AggregateType, opt => opt.MapFrom(s => s.Aggregate.AggregateType));
        }
    }
}
