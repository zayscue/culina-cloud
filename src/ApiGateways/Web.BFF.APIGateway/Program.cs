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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins(configuration["AllowedHosts"])
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = configuration["Auth0:Domain"];
    options.Audience = configuration["Auth0:Audience"];
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("read:tags", policy =>
        policy.RequireClaim("permissions", "read:tags"));
    options.AddPolicy("read:ingredients", policy =>
        policy.RequireClaim("permissions", "read:ingredients"));
    options.AddPolicy("create:image", policy =>
        policy.RequireClaim("permissions", "create:image"));
    options.AddPolicy("read:statistics", policy =>
        policy.RequireClaim("permissions", "read:statistics"));
    options.AddPolicy("read:popular_recipes", policy =>
        policy.RequireClaim("permissions", "read:popular_recipes"));
    options.AddPolicy("create:recipe", policy =>
        policy.RequireClaim("permissions", "create:recipe"));
    options.AddPolicy("read:me", policy =>
        policy.RequireClaim("permissions", "read:me"));
    options.AddPolicy("read:recommended_recipes", policy =>
        policy.RequireClaim("permissions", "read:recommended_recipes"));
    options.AddPolicy("read:favorite_recipes", policy =>
        policy.RequireClaim("permissions", "read:favorite_recipes"));
    options.AddPolicy("read:my_recipes", policy =>
        policy.RequireClaim("permissions", "read:my_recipes"));
    options.AddPolicy("read:recent_recipes", policy =>
        policy.RequireClaim("permissions", "read:recent_recipes"));
    options.AddPolicy("read:recipes", policy =>
        policy.RequireClaim("permissions", "read:recipes"));
    options.AddPolicy("read:recipe", policy =>
        policy.RequireClaim("permissions", "read:recipe"));
    options.AddPolicy("update:recipe", policy =>
        policy.RequireClaim("permissions", "update:recipe"));
    options.AddPolicy("create:recipe_nutrition", policy =>
        policy.RequireClaim("permissions", "create:recipe_nutrition"));
    options.AddPolicy("read:recipe_nutrition", policy =>
        policy.RequireClaim("permissions", "read:recipe_nutrition"));
    options.AddPolicy("read:recipe_steps", policy =>
        policy.RequireClaim("permissions", "read:recipe_steps"));
    options.AddPolicy("read:recipe_step", policy =>
        policy.RequireClaim("permissions", "read:recipe_step"));
    options.AddPolicy("update:recipe_steps", policy =>
        policy.RequireClaim("permissions", "update:recipe_steps"));
    options.AddPolicy("read:recipe_ingredients", policy =>
        policy.RequireClaim("permissions", "read:recipe_ingredients"));
    options.AddPolicy("create:recipe_ingredient", policy =>
        policy.RequireClaim("permissions", "create:recipe_ingredient"));
    options.AddPolicy("update:recipe_ingredients", policy =>
        policy.RequireClaim("permissions", "update:recipe_ingredients"));
    options.AddPolicy("read:recipe_ingredient", policy =>
        policy.RequireClaim("permissions", "read:recipe_ingredient"));
    options.AddPolicy("read:recipe_images", policy =>
        policy.RequireClaim("permissions", "read:recipe_images"));
    options.AddPolicy("create:recipe_image", policy =>
        policy.RequireClaim("permissions", "create:recipe_image"));
    options.AddPolicy("update:recipe_images", policy =>
        policy.RequireClaim("permissions", "update:recipe_images"));
    options.AddPolicy("read:recipe_image", policy =>
        policy.RequireClaim("permissions", "read:recipe_image"));
    options.AddPolicy("read:recipe_tags", policy => 
        policy.RequireClaim("permissions", "read:recipe_tags"));
    options.AddPolicy("create:recipe_tag", policy =>
        policy.RequireClaim("permissions", "create:recipe_tag"));
    options.AddPolicy("update:recipe_tags", policy =>
        policy.RequireClaim("permissions", "update:recipe_tags"));
    options.AddPolicy("read:recipe_tag", policy =>
        policy.RequireClaim("permissions", "read:recipe_tag"));
    options.AddPolicy("read:similar_recipes", policy =>
        policy.RequireClaim("permissions", "read:similar_recipes"));
    options.AddPolicy("read:recipe_reviews", policy =>
        policy.RequireClaim("permissions", "read:recipe_reviews"));
    options.AddPolicy("create:recipe_review", policy =>
        policy.RequireClaim("permissions", "create:recipe_review"));
    options.AddPolicy("read:recipe_entitlements", policy => 
        policy.RequireClaim("permissions", "read:recipe_entitlements"));
    options.AddPolicy("create:recipe_entitlement", policy =>
        policy.RequireClaim("permissions", "create:recipe_entitlement"));
    options.AddPolicy("update:recipe_entitlement", policy =>
        policy.RequireClaim("permissions", "update:recipe_entitlement"));
    options.AddPolicy("create:recipe_favorite", policy =>
        policy.RequireClaim("permissions", "create:recipe_favorite"));
    options.AddPolicy("delete:recipe_favorite", policy =>
        policy.RequireClaim("permissions", "delete:recipe_favorite"));
});
builder.Services.Configure<Auth0Settings>(configuration.GetSection("Auth0"));
builder.Services.Configure<CookBookServiceSettings>(configuration.GetSection("CookBookService"));
builder.Services.Configure<UsersServiceSettings>(configuration.GetSection("UsersService"));
builder.Services.Configure<AnalyticsServiceSettings>(configuration.GetSection("AnalyticsService"));
builder.Services.Configure<InteractionsServiceSettings>(configuration.GetSection("InteractionsService"));
builder.Services.Configure<ImagesServiceSettings>(configuration.GetSection("ImagesService"));
builder.Services.AddHttpContextAccessor();
builder.Services.AddHealthChecks();
builder.Services.AddTransient<IDateTime, DateTimeService>();
builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddTransient<Auth0SecretsProvider, Auth0UserSecretsProvider>(provider =>
    {
        var config = provider.GetService<IConfiguration>();
        return new Auth0UserSecretsProvider(config, "ClientId", "ClientSecret");
    });
}
else
{
    builder.Services.AddTransient<Auth0SecretsProvider, Auth0AWSSecretsProvider>(provider =>
    {
        var secretsManager = new AmazonSecretsManagerClient();
        return new Auth0AWSSecretsProvider(secretsManager, "CulinaCloud/API/OAuthSecrets");
    });
}
builder.Services.AddSingleton<ITokenServiceManager, Auth0TokenServiceManager>(provider =>
{
    var dateTime = provider.GetService<IDateTime>();
    var settings = provider.GetService<IOptions<Auth0Settings>>();
    var secretsProvider = provider.GetService<Auth0SecretsProvider>();
    return new Auth0TokenServiceManager(dateTime, settings, secretsProvider);
});
builder.Services.AddHttpClient<ICookBookService, CookBookService>((client, provider) =>
{
    var settings = provider.GetService<IOptions<CookBookServiceSettings>>();
    var baseAddress = new Uri(settings?.Value.BaseAddress ?? string.Empty);
    client.BaseAddress = baseAddress;
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    var audience = configuration["CookBookService:Audience"];
    var tokenServiceManager = provider.GetService<ITokenServiceManager>();
    var tokenService = tokenServiceManager.GetTokenService(audience);
    return new CookBookService(client, tokenService);
});
builder.Services.AddHttpClient<IUsersService, UsersService>((client, provider) =>
{
    var settings = provider.GetService<IOptions<UsersServiceSettings>>();
    var baseAddress = new Uri(settings?.Value.BaseAddress ?? string.Empty);
    client.BaseAddress = baseAddress;
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    var audience = configuration["UsersService:Audience"];
    var tokenServiceManager = provider.GetService<ITokenServiceManager>();
    var tokenService = tokenServiceManager.GetTokenService(audience);
    return new UsersService(client, tokenService);
});
builder.Services.AddHttpClient<IAnalyticsService, AnalyticsService>((client, provider) =>
{
    var config = provider.GetService<IConfiguration>();
    var clientId = config?["ClientId"];
    var settings = provider.GetService<IOptions<AnalyticsServiceSettings>>();
    var baseAddress = new Uri(settings?.Value.BaseAddress ?? string.Empty);
    client.BaseAddress = baseAddress;
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    var audience = configuration["AnalyticsService:Audience"];
    var tokenServiceManager = provider.GetService<ITokenServiceManager>();
    var tokenService = tokenServiceManager.GetTokenService(audience);
    return new AnalyticsService(client, clientId, tokenService);
});
builder.Services.AddHttpClient<IInteractionsService, InteractionsService>((client, provider) =>
{
    var settings = provider.GetService<IOptions<InteractionsServiceSettings>>();
    var baseAddress = new Uri(settings?.Value.BaseAddress ?? string.Empty);
    client.BaseAddress = baseAddress;
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    var audience = configuration["InteractionsService:Audience"];
    var tokenServiceManager = provider.GetService<ITokenServiceManager>();
    var tokenService = tokenServiceManager.GetTokenService(audience);
    return new InteractionsService(client, tokenService);
});
builder.Services
    .AddControllers()
    .AddJsonOptions((options) =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors();
app.UseHttpsRedirection();
app.UsePathBase(new PathString("/app"));
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health");
    endpoints.MapControllers();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
