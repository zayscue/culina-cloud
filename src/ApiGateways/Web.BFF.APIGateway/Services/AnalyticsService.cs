using System.Text;

namespace CulinaCloud.Web.BFF.APIGateway.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly HttpClient _httpClient;
    private readonly string _clientId;
    private const string ServiceName = "Analytics";

    public AnalyticsService(HttpClient httpClient, string? clientId)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        if (clientId == null)
        {
            throw new ArgumentNullException(nameof(clientId));
        }
        _clientId = $"{clientId}@clients";
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

    public async Task<PaginatedDto<RecipePopularityDto>> GetPopularRecipesAsync(string orderBy, int page, int limit, CancellationToken cancellation = default)
    {
        using var urlContent = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
        {
            new ("orderBy", orderBy),
            new ("page", page.ToString()),
            new ("limit", limit.ToString())
        });
        var query = await urlContent.ReadAsStringAsync(cancellation);
        using var request =
            new HttpRequestMessage(HttpMethod.Get,
                $"/analytics/recipe-popularity?{query}");
        using var response = await _httpClient.SendAsync(request, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var popularRecipes = JsonSerializer.Deserialize<PaginatedDto<RecipePopularityDto>>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new PaginatedDto<RecipePopularityDto>();
        return popularRecipes;
    }

    public async Task<RecipePopularityDto> CreateRecipePopularityStatAsync(Guid recipeId, CancellationToken cancellation = default)
    {
        var json = JsonSerializer.Serialize(
            new
            {
                RecipeId = recipeId,
                CreatedBy = _clientId
            },
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
        using var response = await _httpClient.PostAsync("/analytics/recipe-popularity", requestContent, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var createdRecipePopularityStat = JsonSerializer.Deserialize<RecipePopularityDto>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new RecipePopularityDto();
        return createdRecipePopularityStat;
    }

    public async Task<RecipePopularityDto> UpdateRecipePopularityStatAsync(Guid recipeId, int rating, CancellationToken cancellation = default)
    {
        var json = JsonSerializer.Serialize(
            new
            {
                RecipeId = recipeId,
                Rating = rating,
                LastModifiedBy = _clientId
            },
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
        using var response = await _httpClient.PutAsync("/analytics/recipe-popularity", requestContent, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var updatedRecipePopularityStat = JsonSerializer.Deserialize<RecipePopularityDto>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new RecipePopularityDto();
        return updatedRecipePopularityStat;
    }

    public async Task<RecipePopularityStatisticsDto> GetRecipePopularityStatisticsAsync(CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get,
            "/analytics/statistics");
        using var response = await _httpClient.SendAsync(request, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        var recipePopularityStatistics = JsonSerializer.Deserialize<RecipePopularityStatisticsDto>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new RecipePopularityStatisticsDto();
        return recipePopularityStatistics;
    }

    public async Task<PaginatedDto<RecentRecipeDto>> GetRecentRecipesAsync(int page, int limit, CancellationToken cancellation = default)
    {
        using var urlContent = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
        {
            new ("page", page.ToString()),
            new ("limit", limit.ToString())
        });
        var query = await urlContent.ReadAsStringAsync(cancellation);
        using var request =
            new HttpRequestMessage(HttpMethod.Get,
                $"/analytics/recent-recipes?{query}");
        using var response = await _httpClient.SendAsync(request, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var recentRecipes = JsonSerializer.Deserialize<PaginatedDto<RecentRecipeDto>>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new PaginatedDto<RecentRecipeDto>();
        return recentRecipes;
    }
}