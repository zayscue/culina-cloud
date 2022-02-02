var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<CookBookServiceSettings>(builder.Configuration.GetSection("CookBookService"));
builder.Services.Configure<UsersServiceSettings>(builder.Configuration.GetSection("UsersService"));
builder.Services.Configure<AnalyticsServiceSettings>(builder.Configuration.GetSection("AnalyticsService"));
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
builder.Services
    .AddControllers()
    .AddJsonOptions((options) =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.Run();
