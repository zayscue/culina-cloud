var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IDateTime, DateTimeService>();
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

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.MapGet("/cookbook/recipes/{id:guid}", async (Guid id) =>
{
    return "Hello World!";
});

app.Run();
