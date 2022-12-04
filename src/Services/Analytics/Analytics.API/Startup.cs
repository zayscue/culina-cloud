using CulinaCloud.Analytics.API.Middleware;
using CulinaCloud.Analytics.Application;
using CulinaCloud.Analytics.Infrastructure;
using CulinaCloud.Analytics.Infrastructure.Persistence;
using CulinaCloud.BuildingBlocks.Authorization.HasScope;
using CulinaCloud.BuildingBlocks.CurrentUser;
using CulinaCloud.BuildingBlocks.CurrentUser.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CulinaCloud.Analytics.API
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
            // services.AddAuthorization(options =>
            // {
            //     options.AddPolicy("CreateRecipePopularity", policy =>
            //         policy.Requirements.Add(new HasScopeRequirement("create:recipe_popularity")));
            //     options.AddPolicy("CreateRecipeSimilarity", policy =>
            //         policy.Requirements.Add(new HasScopeRequirement("create:recipe_similarity")));
            //     options.AddPolicy("ReadPersonalRecipeRecommendations", policy =>
            //         policy.Requirements.Add(new HasScopeRequirement("read:personal_recipe_recommendations")));
            //     options.AddPolicy("ReadSimilarRecipes", policy =>
            //         policy.Requirements.Add(new HasScopeRequirement("read:similar_recipes")));
            // });
            //services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
            services.AddApplication();
            services.AddInfrastructure(Configuration, Environment.IsDevelopment());
            services.AddHttpContextAccessor();
            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddControllers();
            services.AddResponseCompression();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Analytics.API", Version = "v1" });
            });
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");
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
                c.RouteTemplate = "analytics/swagger/{documentname}/swagger.json";
            });
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/analytics/swagger/v1/swagger.json", "Analytics.API v1");
                c.RoutePrefix = "analytics/swagger";
            });
        }
    }
}
