using System.Text;

namespace CulinaCloud.Web.BFF.APIGateway.Services;

public class UsersService : IUsersService
{
    private const string ServiceName = "Users";
    private readonly HttpClient _httpClient;

    public UsersService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<PaginatedDto<FavoriteDto>> GetUsersFavoritesAsync(string userId, int page, int limit,
        CancellationToken cancellation = default)
    {
        using var urlContent = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
        {
            new ("page", page.ToString()),
            new ("limit", limit.ToString()),
            new ("userId", userId)
        });
        var query = await urlContent.ReadAsStringAsync(cancellation);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/users/favorites?{query}");
        using var response = await _httpClient.SendAsync(request, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var favoritesPaginatedList = JsonSerializer.Deserialize<PaginatedDto<FavoriteDto>>(responseContent, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }) ?? new PaginatedDto<FavoriteDto>();
        favoritesPaginatedList.Items ??= new List<FavoriteDto>();
        return favoritesPaginatedList;
    }

    public async Task<PaginatedDto<FavoriteDto>> GetUsersFavoritesAsync(string userId, List<Guid> recipeIds, int page,
        int limit, CancellationToken cancellation = default)
    {
        var urlParams = new List<KeyValuePair<string, string>>
        {
            new ("page", page.ToString()),
            new ("limit", limit.ToString()),
            new ("userId", userId)
        };
        urlParams.AddRange(recipeIds.Select(recipeId =>
            new KeyValuePair<string, string>("recipeIds", recipeId.ToString())));
        using var urlContent = new FormUrlEncodedContent(urlParams);
        var query = await urlContent.ReadAsStringAsync(cancellation);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/users/favorites?{query}");
        using var response = await _httpClient.SendAsync(request, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var favoritesPaginatedList = JsonSerializer.Deserialize<PaginatedDto<FavoriteDto>>(responseContent, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }) ?? new PaginatedDto<FavoriteDto>();
        favoritesPaginatedList.Items ??= new List<FavoriteDto>();
        return favoritesPaginatedList;
    }

    public async Task<FavoriteDto> CreateFavoriteAsync(FavoriteDto favorite, CancellationToken cancellation = default)
    {
        var jsonStr = JsonSerializer.Serialize(favorite, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        using var requestBodyContent = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        using var request = new HttpRequestMessage(HttpMethod.Post, "/users/favorites")
        {
            Content = requestBodyContent
        };
        using var response = await _httpClient.SendAsync(request, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var createdFavorite = JsonSerializer.Deserialize<FavoriteDto>(responseContent, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }) ?? new FavoriteDto();
        return createdFavorite;
    }

    public async Task DeleteFavoriteAsync(FavoriteDto favorite, CancellationToken cancellation = default)
    {
        var jsonStr = JsonSerializer.Serialize(favorite, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        using var requestBodyContent = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        using var request = new HttpRequestMessage(HttpMethod.Delete, "/users/favorites")
        {
            Content = requestBodyContent
        };
        using var response = await _httpClient.SendAsync(request, cancellation);
    }

    public async Task<PaginatedDto<RecipeEntitlementDto>> GetRecipeEntitlementsAsync(Guid recipeId, string userId, CancellationToken cancellation = default)
    {
        var urlParams = new List<KeyValuePair<string, string>>
        {
            new ("userId", userId),
            new ("recipeId", recipeId.ToString())
        };
        using var urlContent = new FormUrlEncodedContent(urlParams);
        var query = await urlContent.ReadAsStringAsync(cancellation);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/users/recipe-entitlements?{query}");
        using var response = await _httpClient.SendAsync(request, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var recipeEntitlementsPaginatedList = JsonSerializer.Deserialize<PaginatedDto<RecipeEntitlementDto>>(responseContent, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }) ?? new PaginatedDto<RecipeEntitlementDto>();
        recipeEntitlementsPaginatedList.Items ??= new List<RecipeEntitlementDto>();
        return recipeEntitlementsPaginatedList;
    }

    public async Task<RecipeEntitlementDto> CreateRecipeEntitlementAsync(RecipeEntitlementDto recipeEntitlement, CancellationToken cancellation = default)
    {
        var jsonStr = JsonSerializer.Serialize(recipeEntitlement, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        using var requestBodyContent = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        using var request = new HttpRequestMessage(HttpMethod.Post, "/users/recipe-entitlements")
        {
            Content = requestBodyContent
        };
        using var response = await _httpClient.SendAsync(request, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var createdRecipeEntitlement = JsonSerializer.Deserialize<RecipeEntitlementDto>(responseContent, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }) ?? new RecipeEntitlementDto();
        return createdRecipeEntitlement;
    }

    public async Task<bool> UpdateRecipeEntitlementAsync(Guid recipeEntitlementId, RecipeEntitlementDto recipeEntitlement, CancellationToken cancellation = default)
    {
        var jsonStr = JsonSerializer.Serialize(recipeEntitlement, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        using var requestBodyContent = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        using var request = new HttpRequestMessage(HttpMethod.Put, $"/users/recipe-entitlements/{recipeEntitlementId}")
        {
            Content = requestBodyContent
        };
        using var response = await _httpClient.SendAsync(request, cancellation);
        return response.IsSuccessStatusCode;
    }

    public async Task<PaginatedDto<RecipeEntitlementDto>> GetRecipeEntitlementsAsync(List<Guid> recipeIds, string userId, CancellationToken cancellation = default)
    {
        var urlParams = new List<KeyValuePair<string, string>>
        {
            new ("limit", recipeIds.Count.ToString()),
            new ("userId", userId)
        };
        urlParams.AddRange(recipeIds.Select(recipeId =>
            new KeyValuePair<string, string>("recipeIds", recipeId.ToString())));
        using var urlContent = new FormUrlEncodedContent(urlParams);
        var query = await urlContent.ReadAsStringAsync(cancellation);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/users/recipe-entitlements?{query}");
        using var response = await _httpClient.SendAsync(request, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var recipeEntitlementsPaginatedList = JsonSerializer.Deserialize<PaginatedDto<RecipeEntitlementDto>>(responseContent, 
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new PaginatedDto<RecipeEntitlementDto>();
        recipeEntitlementsPaginatedList.Items ??= new List<RecipeEntitlementDto>();
        return recipeEntitlementsPaginatedList;
    }

    public async Task<bool> DeleteRecipeEntitlementAsync(Guid recipeEntitlementId, RecipeEntitlementDto recipeEntitlement, CancellationToken cancellation = default)
    {
        var jsonStr = JsonSerializer.Serialize(recipeEntitlement, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        using var requestBodyContent = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"/users/recipe-entitlements/{recipeEntitlementId}")
        {
            Content = requestBodyContent
        };
        using var response = await _httpClient.SendAsync(request, cancellation);
        return response.IsSuccessStatusCode;
    }

    public async Task<PaginatedDto<RecipeEntitlementDto>> GetRecipeEntitlementsAsync(Guid recipeId, List<string> userIds, CancellationToken cancellation = default)
    {
        var urlParams = new List<KeyValuePair<string, string>>
        {
            new ("limit", userIds.Count.ToString()),
            new ("recipeId", recipeId.ToString())
        };
        urlParams.AddRange(userIds.Select(userId =>
            new KeyValuePair<string, string>("userIds", userId.ToString())));
        using var urlContent = new FormUrlEncodedContent(urlParams);
        var query = await urlContent.ReadAsStringAsync(cancellation);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/users/recipe-entitlements?{query}");
        using var response = await _httpClient.SendAsync(request, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var recipeEntitlementsPaginatedList = JsonSerializer.Deserialize<PaginatedDto<RecipeEntitlementDto>>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new PaginatedDto<RecipeEntitlementDto>();
        recipeEntitlementsPaginatedList.Items ??= new List<RecipeEntitlementDto>();
        return recipeEntitlementsPaginatedList;
    }

    public async Task<PaginatedDto<RecipeEntitlementDto>> GetRecipeEntitlementsAsync(Guid recipeId, CancellationToken cancellation = default)
    {
        var urlParams = new List<KeyValuePair<string, string>>
        {
            new ("recipeId", recipeId.ToString())
        };
        using var urlContent = new FormUrlEncodedContent(urlParams);
        var query = await urlContent.ReadAsStringAsync(cancellation);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/users/recipe-entitlements?{query}");
        using var response = await _httpClient.SendAsync(request, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var recipeEntitlementsPaginatedList = JsonSerializer.Deserialize<PaginatedDto<RecipeEntitlementDto>>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new PaginatedDto<RecipeEntitlementDto>();
        recipeEntitlementsPaginatedList.Items ??= new List<RecipeEntitlementDto>();
        return recipeEntitlementsPaginatedList;
    }
}