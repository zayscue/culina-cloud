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
});
builder.Services.Configure<Auth0Settings>(configuration.GetSection("Auth0"));
builder.Services.Configure<CookBookServiceSettings>(configuration.GetSection("CookBookService"));
builder.Services.Configure<UsersServiceSettings>(configuration.GetSection("UsersService"));
builder.Services.Configure<AnalyticsServiceSettings>(configuration.GetSection("AnalyticsService"));
builder.Services.Configure<InteractionsServiceSettings>(configuration.GetSection("InteractionsService"));
builder.Services.Configure<ImagesServiceSettings>(configuration.GetSection("ImagesService"));
builder.Services.AddHttpContextAccessor();
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

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
