﻿using CulinaCloud.BuildingBlocks.CurrentUser.Abstractions;
using CulinaCloud.Users.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace CulinaCloud.Users.Infrastructure.Persistence
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Database=CulinaCloudDB;Username=postgres;Password=postgres");

            return new ApplicationDbContext(
                optionsBuilder.Options,
                new LoggerFactory(),
                new DesignTimeCurrentUserService(),
                new DateTimeService()
            );
        }
    }

    public class DesignTimeCurrentUserService : ICurrentUserService
    {
        public string UserId => throw new System.NotImplementedException();
    }
}
