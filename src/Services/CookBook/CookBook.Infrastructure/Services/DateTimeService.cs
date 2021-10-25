using CulinaCloud.BuildingBlocks.Common.Interfaces;
using System;

namespace CulinaCloud.CookBook.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
