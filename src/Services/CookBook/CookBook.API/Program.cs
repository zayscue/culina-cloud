using System;
using System.IO;
using Serilog;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Culina.CookBook.Application;
using Culina.CookBook.Application.Common.Interfaces;
using Culina.CookBook.Infrastructure;
using Culina.CookBook.API.Services;
using Culina.CookBook.API.Actions;
using Culina.CookBook.API.Extensions;
using Culina.CookBook.Infrastructure.Persistence;

static IConfiguration GetConfiguration()
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
        .ConfigureAppConfiguration((WebHostBuilderContext webHostBuilderContext, IConfigurationBuilder configuration) =>
        {
            var env = webHostBuilderContext.HostingEnvironment.EnvironmentName;
            configuration.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        })
        .UseSerilog()
        .ConfigureServices((WebHostBuilderContext webHostBuilderContext, IServiceCollection services)  =>
        {
            services.AddApplication();
            services.AddInfrastructure(webHostBuilderContext.Configuration);
            services.AddHttpContextAccessor();
            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
        })
        .Configure((WebHostBuilderContext webHostBuilderContext, IApplicationBuilder app) =>
        {
            var env = webHostBuilderContext.HostingEnvironment;
            app.ConfigureExceptionHandler(env);

            app.UseStaticFiles();
            
            app.UseRouting();
            app.UseEndpoints(e =>
            {
                e.MapHealthChecks("/health");
                e.MapPost("/ingredients", PostIngredients.Perform);
                e.MapGet("/ingredients", GetIngredients.Perform);
                e.MapPost("/tags", PostTags.Perform);
                e.MapGet("/tags", GetTags.Perform);
                e.MapPost("/recipes", PostRecipes.Perform);
                e.MapGet("/recipes/{recipeId:guid}", GetRecipe.Perform);
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "CookBook API");
            });
        })
        .Build()
        .MigrateDbContext<ApplicationDbContext>()
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
