using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using MediatR;
using CulinaCloud.EventStore.Application;
using CulinaCloud.EventStore.Infrastructure;
using CulinaCloud.EventStore.Application.Events.Commands.StoreEvent;
using CulinaCloud.EventStore.Application.Common.Interfaces;

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

WebHost.CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
        services.AddApplication();
        services.AddInfrastructure(configuration);
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

            e.MapPost("/store/{aggregateId:guid}", async context =>
            {
                var mediator = context.RequestServices.GetRequiredService<ISender>();
                var aggregateId = new Guid(context.Request.RouteValues["aggregateId"].ToString());
                var genericAggregateEvents = await context.Request.ReadFromJsonAsync<List<GenericAggregateEvent>>();
                var storeEventCommand = new StoreEventCommand
                {
                    AggregateId = aggregateId,
                    Events = genericAggregateEvents
                };
                await mediator.Send(storeEventCommand);
                context.Response.StatusCode = StatusCodes.Status201Created;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new 
                {
                    AggregateId = aggregateId
                });
            });
        });
    })
    .Build()
    .Run();
