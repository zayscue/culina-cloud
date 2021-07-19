using CulinaCloud.BuildingBlocks.CurrentUser;
using CulinaCloud.BuildingBlocks.CurrentUser.Abstractions;
using CulinaCloud.BuildingBlocks.PostMaster;
using CulinaCloud.BuildingBlocks.PostMaster.Abstractions;
using CulinaCloud.BuildingBlocks.PostMaster.BackgroundService;
using CulinaCloud.Interactions.API.Extensions;
using CulinaCloud.Interactions.Application;
using CulinaCloud.Interactions.Application.Interfaces;
using CulinaCloud.Interactions.Infrastructure;
using CulinaCloud.Interactions.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CulinaCloud.Interactions.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get;  }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = Configuration["Auth0:Domain"];
                options.Audience = Configuration["Auth0:Audience"];
            });
            services.AddApplication();
            services.AddInfrastructure(Configuration, Environment.IsDevelopment());
            services.Configure<PostMasterBackgroundServiceSettings>(
                Configuration.GetSection("PostMaster:BackgroundServiceSettings"));
            services.AddScoped<IEventOutboxDbContext>(provider => provider.GetService<IApplicationDbContext>());
            services.AddScoped<IDeliveryService, DeliveryService>();
            services.AddHostedService<PostMasterBackgroundService>();
            services.AddHttpContextAccessor();
            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddControllers();
            services.AddResponseCompression();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Interactions.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();
            app.ConfigureExceptionHandler(env);
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
            });

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "interactions/swagger/{documentname}/swagger.json";
            });
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/interactions/swagger/v1/swagger.json", "Interactions.API v1");
                c.RoutePrefix = "interactions/swagger";
            });
        }
    }
}
