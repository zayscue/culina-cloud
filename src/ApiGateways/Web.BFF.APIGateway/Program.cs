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
builder.Services.Configure<CookBookServiceSettings>(configuration.GetSection("CookBookService"));
builder.Services.Configure<UsersServiceSettings>(configuration.GetSection("UsersService"));
builder.Services.Configure<AnalyticsServiceSettings>(configuration.GetSection("AnalyticsService"));
builder.Services.Configure<InteractionsServiceSettings>(configuration.GetSection("InteractionsService"));
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IDateTime, DateTimeService>();
builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
// if (builder.Environment.IsDevelopment())
// {
//     builder.Services.AddTransient<Auth0SecretsProvider, Auth0UserSecretsProvider>(provider =>
//     {
//         var config = provider.GetService<IConfiguration>();
//         return new Auth0UserSecretsProvider(config, "ClientId", "ClientSecret");
//     });
// }
// else
// {
//     builder.Services.AddTransient<Auth0SecretsProvider, Auth0AWSSecretsProvider>(provider =>
//     {
//         var secretsManager = new AmazonSecretsManagerClient();
//         return new Auth0AWSSecretsProvider(secretsManager, "CulinaCloud/API/OAuthSecrets");
//     });
// }
// builder.Services.AddSingleton<ITokenServiceManager, Auth0TokenServiceManager>(provider =>
// {
//     var dateTime = provider.GetService<IDateTime>();
//     var settings = provider.GetService<IOptions<Auth0Settings>>();
//     var secretsProvider = provider.GetService<Auth0SecretsProvider>();
//     return new Auth0TokenServiceManager(dateTime, settings, secretsProvider);
// });
builder.Services.AddHttpClient<ICookBookService, CookBookService>((client, provider) =>
{
    var settings = provider.GetService<IOptions<CookBookServiceSettings>>();
    var baseAddress = new Uri(settings?.Value.BaseAddress ?? string.Empty);
    client.BaseAddress = baseAddress;
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    return new CookBookService(client);
});
builder.Services.AddHttpClient<IUsersService, UsersService>((client, provider) =>
{
    var settings = provider.GetService<IOptions<UsersServiceSettings>>();
    var baseAddress = new Uri(settings?.Value.BaseAddress ?? string.Empty);
    client.BaseAddress = baseAddress;
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    return new UsersService(client);
});
builder.Services.AddHttpClient<IAnalyticsService, AnalyticsService>((client, provider) =>
{
    var settings = provider.GetService<IOptions<AnalyticsServiceSettings>>();
    var baseAddress = new Uri(settings?.Value.BaseAddress ?? string.Empty);
    client.BaseAddress = baseAddress;
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    return new AnalyticsService(client);
});
builder.Services.AddHttpClient<IInteractionsService, InteractionsService>((client, provider) =>
{
    var settings = provider.GetService<IOptions<InteractionsServiceSettings>>();
    var baseAddress = new Uri(settings?.Value.BaseAddress ?? string.Empty);
    client.BaseAddress = baseAddress;
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    return new InteractionsService(client);
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
