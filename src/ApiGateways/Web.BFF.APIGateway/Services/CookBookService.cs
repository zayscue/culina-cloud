namespace CulinaCloud.Web.BFF.APIGateway.Services;

public class CookBookService : ICookBookService
{
    private readonly ITokenService _tokenService;
    private readonly HttpClient _httpClient;

    public CookBookService(ITokenService tokenService, HttpClient httpClient)
    {
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<RecipeDto> GetRecipeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var (tokenType, accessToken) = await _tokenService.GetToken(cancellationToken);
        if (string.IsNullOrEmpty(tokenType) || string.IsNullOrEmpty(accessToken))
        {
            throw new Exception("Couldn't retrieve the access token for the cookbook service");
        }
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(baseUri: _httpClient.BaseAddress, relativeUri: $"/cookbook/recipes/${id}"),
            Headers =
            {
                { HttpRequestHeader.Authorization.ToString(), $"{tokenType} {accessToken}" }
            }
        };
        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode) throw new Exception("Couldn't retrieve the recipe");
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        var recipe = JsonSerializer.Deserialize<RecipeDto>(responseContent, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        return recipe;
    }
}
