namespace CulinaCloud.Web.BFF.APIGateway.Services;

public class AnalyticsService : IAnalyticsService
{
    private const string ServiceName = "Analytics";
    private readonly HttpClient _httpClient;

    public AnalyticsService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<PaginatedDto<RecipeRecommendationDto>> GetPersonalizedRecipeRecommendationsAsync(string userId, 
        int page, int limit, CancellationToken cancellation = default)
    {
        
        using var urlContent = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
        {
            new ("userId", userId),
            new ("limit", limit.ToString()),
            new ("page", page.ToString())
        });
        var query = await urlContent.ReadAsStringAsync(cancellation);
        using var request =
            new HttpRequestMessage(HttpMethod.Get,
                $"/analytics/recommendations/personal-recipe-recommendations?{query}");
        using var response = await _httpClient.SendAsync(request, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var recipeRecommendations = JsonSerializer.Deserialize<PaginatedDto<RecipeRecommendationDto>>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new PaginatedDto<RecipeRecommendationDto>();
        return recipeRecommendations;
    }

    public async Task<PaginatedDto<RecipeSimilarityDto>> GetSimilarRecipesAsync(Guid recipeId, int page, int limit, CancellationToken cancellation = default)
    {
        using var urlContent = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
        {
            new ("recipeId", recipeId.ToString()),
            new ("page", page.ToString()),
            new ("limit", limit.ToString())
        });
        var query = await urlContent.ReadAsStringAsync(cancellation);
        using var request = 
            new HttpRequestMessage(HttpMethod.Get,
                $"/analytics/recommendations/similar-recipes?{query}");
        using var response = await _httpClient.SendAsync(request, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var similarRecipes = JsonSerializer.Deserialize<PaginatedDto<RecipeSimilarityDto>>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new PaginatedDto<RecipeSimilarityDto>();
        return similarRecipes;
    }
}