using System;
using System.IO;
using System.Text.Json;
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
using Culina.CookBook.Application.Recipes.Queries.GetRecipe;
using Culina.CookBook.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;

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
        .ConfigureAppConfiguration((WebHostBuilderContext webHostBuilderContext, IConfigurationBuilder config) =>
        {
            var env = webHostBuilderContext.HostingEnvironment.EnvironmentName;
            config.SetBasePath(Directory.GetCurrentDirectory())
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
            services.AddControllers();
            services.AddResponseCompression();
        })
        .Configure((WebHostBuilderContext webHostBuilderContext, IApplicationBuilder app) =>
        {
            var env = webHostBuilderContext.HostingEnvironment;
            app.UseResponseCompression();
            app.ConfigureExceptionHandler(env);

            app.UseStaticFiles();
            
            app.UseRouting();
            app.UseEndpoints(e =>
            {
                e.MapHealthChecks("/health");
                e.MapControllers();
                e.MapPost("/ingredients", PostIngredients.Perform);
                e.MapGet("/ingredients", GetIngredients.Perform);
                e.MapPost("/tags", PostTags.Perform);
                e.MapGet("/tags", GetTags.Perform);
                e.MapPost("/recipes", PostRecipes.Perform);
                e.MapGet("/recipes/{recipeId:guid}", async context =>
                {
                    var mediator = context.RequestServices.GetRequiredService<ISender>();
                    var recipeId = new Guid(context.Request.RouteValues["recipeId"]?.ToString() ?? string.Empty);
                    var query = new GetRecipeQuery()
                    {
                        Id = recipeId
                    };
                    var response = await mediator.Send(query);
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    await context.Response.WriteAsJsonAsync(response, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        IgnoreNullValues = true
                    });
                });
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
