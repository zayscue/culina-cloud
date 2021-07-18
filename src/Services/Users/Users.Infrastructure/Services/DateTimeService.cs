using CulinaCloud.BuildingBlocks.Common.Interfaces;
using System;

namespace CulinaCloud.Users.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
