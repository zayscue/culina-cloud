using System;
using CulinaCloud.CookBook.Application.Common.Interfaces;

namespace CulinaCloud.CookBook.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
