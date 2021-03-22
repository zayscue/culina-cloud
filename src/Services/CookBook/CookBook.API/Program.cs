using System;
using System.IO;
using Serilog;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Culina.CookBook.Application;
using Culina.CookBook.Application.Common.Interfaces;
using Culina.CookBook.Infrastructure;
using Culina.CookBook.API.Services;
using Culina.CookBook.API.Actions;
using Culina.CookBook.API.Extensions;
using Culina.CookBook.Infrastructure.Persistence;

IConfiguration GetConfiguration()
{
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    return new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();
}

var configuration = GetConfiguration();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    Log.Information("Starting web host");
    WebHost.CreateDefaultBuilder()
        .UseSerilog()
        .ConfigureServices(services =>
        {
            services.AddApplication();
            services.AddInfrastructure(configuration);
            services.AddHttpContextAccessor();
            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
        })
        .Configure((WebHostBuilderContext webHostBuilderContext, IApplicationBuilder app) =>
        {
            var env = webHostBuilderContext.HostingEnvironment;
            app.ConfigureExceptionHandler(env);
            app.UseRouting();
            app.UseEndpoints(e =>
            {
                e.MapHealthChecks("/health");
                e.MapPost("/ingredients", PostIngredient.Perform);
                e.MapGet("/ingredients", GetIngredients.Perform);
            });
        })
        .Build()
        .Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
