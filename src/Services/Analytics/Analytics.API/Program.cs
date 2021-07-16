using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using CulinaCloud.Analytics.Infrastructure.Persistence;
using CulinaCloud.BuildingBlocks.WebHost.Customization;

namespace CulinaCloud.Analytics.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .MigrateDbContext<ApplicationDbContext>()
                .Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}