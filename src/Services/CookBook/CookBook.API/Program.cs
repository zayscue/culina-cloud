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
using Culina.CookBook.API.Extensions;
using Culina.CookBook.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Hosting;
using Culina.CookBook.API.BackgroundServices;

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
            if (webHostBuilderContext.HostingEnvironment.IsDevelopment())
            {
                config.AddUserSecrets<Culina.CookBook.API.Controllers.ApiControllerBase>();
            }
        })
        .UseSerilog()
        .ConfigureServices((WebHostBuilderContext webHostBuilderContext, IServiceCollection services)  =>
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
            services.AddInfrastructure(webHostBuilderContext.Configuration,
                webHostBuilderContext.HostingEnvironment.IsDevelopment());
            services.Configure<EventDeliveryBackgroundServiceSettings>(
                webHostBuilderContext.Configuration.GetSection("EventDeliveryBackgroundService"));
            services.AddHostedService<EventDeliveryBackgroundService>();
            services.AddScoped<IEventDeliveryService, EventDeliveryService>();
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
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(e =>
            {
                e.MapHealthChecks("/health");
                e.MapControllers();
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(url: "/cookbook/swagger/v1/swagger.json", name: "CookBook API");
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
