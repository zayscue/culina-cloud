using System;
using System.IO;
using System.Collections.Generic;
using CulinaCloud.EventStore.API.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using MediatR;
using CulinaCloud.EventStore.Application;
using CulinaCloud.EventStore.Infrastructure;
using CulinaCloud.EventStore.Application.Events.Commands.StoreEvent;
using CulinaCloud.EventStore.Application.Events.Queries.LoadEvents;
using CulinaCloud.EventStore.Application.Common.Exceptions;
using CulinaCloud.EventStore.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;

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
        .ConfigureAppConfiguration((WebHostBuilderContext webHostBuilderContext, IConfigurationBuilder config) =>
        {
            var env = webHostBuilderContext.HostingEnvironment.EnvironmentName;
            config.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        })
        .UseSerilog()
        .ConfigureServices((WebHostBuilderContext webHostBuilderContext, IServiceCollection services) =>
        {
            services.AddAuthentication(options =>
            { 
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = webHostBuilderContext.Configuration["Auth0:Domain"];
                options.Audience = webHostBuilderContext.Configuration["Auth0:Audience"];
            });
            services.AddApplication();
            services.AddInfrastructure(configuration);
            services.AddHttpContextAccessor();
            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();
            services.AddControllers();
            services.AddResponseCompression();
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
        })
        .Configure((WebHostBuilderContext webHostBuilderContext, IApplicationBuilder app) =>
        {
            var env = webHostBuilderContext.HostingEnvironment;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("CorsPolicy");
            app.UseResponseCompression();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(e =>
            {
                e.MapHealthChecks("/health");

                e.MapPost("/eventstore/store/{aggregateId:guid}", async context =>
                {
                    var mediator = context.RequestServices.GetRequiredService<ISender>();
                    var aggregateId = new Guid(context.Request.RouteValues["aggregateId"].ToString());
                    var genericAggregateEvents = await context.Request.ReadFromJsonAsync<List<GenericAggregateEvent>>();
                    var storeEventCommand = new StoreEventCommand
                    {
                        AggregateId = aggregateId,
                        Events = genericAggregateEvents
                    };
                    try
                    {
                        var response = await mediator.Send(storeEventCommand);
                        context.Response.StatusCode = StatusCodes.Status201Created;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsJsonAsync(response);
                    }
                    catch (ValidationException ve)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsJsonAsync(new
                        {
                            ve.Message,
                            ve.Errors
                        });
                    }
                    catch (Exception)
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsJsonAsync(new
                        {
                            Message = "An unexpected error occurred."
                        });
                    }
                }).RequireAuthorization();

                e.MapGet("/eventstore/load/{aggregateId:guid}", async context =>
                {
                    var mediator = context.RequestServices.GetRequiredService<ISender>();
                    var aggregateId = new Guid(context.Request.RouteValues["aggregateId"].ToString());
                    var loadEventsQuery = new LoadEventsQuery
                    {
                        AggregateId = aggregateId
                    };
                    var response = await mediator.Send(loadEventsQuery);
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(response);
                }).RequireAuthorization();
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
