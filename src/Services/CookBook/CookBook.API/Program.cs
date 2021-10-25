using CulinaCloud.BuildingBlocks.WebHost.Customization;
using CulinaCloud.CookBook.Infrastructure.Persistence;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace CulinaCloud.CookBook.API
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