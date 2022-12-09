namespace CulinaCloud.Web.BFF.APIGateway.Settings;

public record UsersServiceSettings
{
    public string BaseAddress { get; set; } = default!;
}