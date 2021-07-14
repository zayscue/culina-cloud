﻿using System;
using CulinaCloud.BuildingBlocks.Common.Interfaces;

namespace CulinaCloud.Analytics.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
