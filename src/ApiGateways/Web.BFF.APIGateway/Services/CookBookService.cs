namespace CulinaCloud.Web.BFF.APIGateway.Services;

public class CookBookService : ICookBookService
{
    private readonly HttpClient _httpClient;
    //private readonly ITokenService tokenService;

    public CookBookService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<RecipeDto?> GetRecipeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        //var (tokenType, accessToken) = await _tokenService.GetToken(cancellationToken);
        //if (string.IsNullOrEmpty(tokenType) || string.IsNullOrEmpty(accessToken))
        //{
        //    throw new Exception("Couldn't retrieve the access token for the cookbook service");
        //}
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/recipes/{id}");
        using var response = await _httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        var recipe = JsonSerializer.Deserialize<RecipeDto>(responseContent, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        return recipe;
    }
}
