using System;
using System.IO;
using Serilog;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CulinaCloud.CookBook.API.BackgroundServices;
using CulinaCloud.CookBook.API.Controllers;
using CulinaCloud.CookBook.API.Extensions;
using CulinaCloud.CookBook.API.Services;
using CulinaCloud.CookBook.Application;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using CulinaCloud.CookBook.Infrastructure;
using CulinaCloud.CookBook.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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
                config.AddUserSecrets<ApiControllerBase>();
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CookBook.API", Version = "v1" });
            });
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
            app.UseCors("CorsPolicy");
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
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "cookbook/swagger/{documentname}/swagger.json";
            });
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/cookbook/swagger/v1/swagger.json", "CookBook.API v1");
                c.RoutePrefix = "cookbook/swagger";
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
