using System;
using Culina.CookBook.Application.Common.Interfaces;

namespace Culina.CookBook.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
