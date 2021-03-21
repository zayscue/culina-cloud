using System;
using System.IO;
using Culina.CookBook.API.Services;
using Serilog;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Culina.CookBook.Application;
using Culina.CookBook.Application.Common.Interfaces;
using Culina.CookBook.Application.Ingredients.Commands.CreateIngredient;
using Culina.CookBook.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

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
        .ConfigureServices(services =>
        {
            services.AddApplication();
            services.AddInfrastructure(configuration);

            services.AddHttpContextAccessor();

            services.AddSingleton<ICurrentUserService, CurrentUserService>();
        })
        .Configure((WebHostBuilderContext webHostBuilderContext, IApplicationBuilder app) =>
        {
            var env = webHostBuilderContext.HostingEnvironment;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(e =>
            {
                e.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });

                e.MapPost("/ingredients", async context =>
                {
                    var mediator = context.RequestServices.GetRequiredService<ISender>();
                    var createIngredientCommand = await context.Request.ReadFromJsonAsync<CreateIngredientCommand>();
                    if (createIngredientCommand == null)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsJsonAsync(new
                        {
                            Message = "An error occurred while reading the request content."
                        });
                    }
                    var response = await mediator.Send(createIngredientCommand!);
                    context.Response.StatusCode = StatusCodes.Status201Created;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(new
                    {
                        Id = response,
                        createIngredientCommand.IngredientName,
                    });
                });
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
