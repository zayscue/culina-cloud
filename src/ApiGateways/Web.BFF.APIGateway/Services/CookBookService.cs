namespace CulinaCloud.Web.BFF.APIGateway.Services;

public class CookBookService : ICookBookService
{
    private const string ServiceName = "CookBook";
    private readonly HttpClient _httpClient;

    public CookBookService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<RecipeDto?> GetRecipeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/recipes/{id}");
        using var response = await _httpClient.SendAsync(request, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            switch(response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new RecipeNotFoundException(id);
                case HttpStatusCode.Unauthorized:
                    throw new InternalServiceAuthorizationException(_httpClient.BaseAddress?.ToString() ?? ServiceName);
                default:
                    throw new InternalServiceException(responseContent);
            }
        }
        var recipe = JsonSerializer.Deserialize<RecipeDto>(responseContent, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        return recipe;
    }
}
