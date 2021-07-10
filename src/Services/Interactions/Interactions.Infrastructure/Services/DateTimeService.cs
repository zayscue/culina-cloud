using System;
using CulinaCloud.BuildingBlocks.Common.Interfaces;

namespace CulinaCloud.Interactions.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;
    }
}