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
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
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
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
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
        var jsonStr = JsonSerializer.Serialize(new
        {
            UserId = recipeEntitlement.UserId,
            RecipeId = recipeEntitlement.RecipeId,
            Type = recipeEntitlement.Type,
            GrantedBy = recipeEntitlement.GrantedBy
        }, 
        new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
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
        var jsonStr = JsonSerializer.Serialize(new
        {
            Id = recipeEntitlement.Id,
            Type = recipeEntitlement.Type,
            GrantedBy = recipeEntitlement.GrantedBy
        },
        new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
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
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });
        using var requestBodyContent = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"/users/recipe-entitlements/{recipeEntitlementId}")
        {
            Content = requestBodyContent
        };
        using var response = await _httpClient.SendAsync(request, cancellation);
        return response.IsSuccessStatusCode;
    }

    public async Task<UserStatisticsDto> GetUserStatisticsAsync(CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync($"/statistics", cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        var userStatistics = JsonSerializer.Deserialize<UserStatisticsDto>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new UserStatisticsDto();
        return userStatistics;
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

    public async Task<List<ApplicationUserPolicyDto>> GetApplicationUserPoliciesAsync(string userId, List<Guid>? recipeIds = null, CancellationToken cancellation = default)
    {
        var urlParams = new List<KeyValuePair<string, string>>();
        if (recipeIds != null)
        {
            urlParams.AddRange(recipeIds.Select(userId =>
                new KeyValuePair<string, string>("recipeIds", userId.ToString())));
        }
        using var urlContent = new FormUrlEncodedContent(urlParams);
        var query = await urlContent.ReadAsStringAsync(cancellation);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/users/{userId}/policies?{query}");
        using var response = await _httpClient.SendAsync(request, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var applicationUserPolicies = JsonSerializer.Deserialize<List<ApplicationUserPolicyDto>>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new List<ApplicationUserPolicyDto>();
        return applicationUserPolicies;
    }

    public async Task<ApplicationUserDto?> GetApplicationUserAsync(string userId, CancellationToken cancellation = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/users/{userId}");
        using var response = await _httpClient.SendAsync(request, cancellation);
        if (!response.IsSuccessStatusCode) return null;
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var applicationUser = JsonSerializer.Deserialize<ApplicationUserDto>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new ApplicationUserDto();
        return applicationUser;
    }

    public async Task<ApplicationUserDto?> FindApplicationUserByEmailAsync(string email, CancellationToken cancellation = default)
    {
        var urlParams = new List<KeyValuePair<string, string>>
        {
            new ("email", email)
        };
        using var urlContent = new FormUrlEncodedContent(urlParams);
        var query = await urlContent.ReadAsStringAsync(cancellation);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/users?{query}");
        using var response = await _httpClient.SendAsync(request, cancellation);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var applicationUsersPaginatedList = JsonSerializer.Deserialize<PaginatedDto<ApplicationUserDto>>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new PaginatedDto<ApplicationUserDto>();
        applicationUsersPaginatedList.Items ??= new List<ApplicationUserDto>();
        return applicationUsersPaginatedList.Items.FirstOrDefault();
    }

    public async Task<PaginatedDto<ApplicationUserDto>> GetApplicationUsersAsync(List<string> userIds, CancellationToken cancellation = default)
    {
        var urlParams = new List<KeyValuePair<string, string>>();
        if (userIds != null)
        {
            urlParams.AddRange(userIds.Select(userId =>
                new KeyValuePair<string, string>("userIds", userId)));
        }
        using var urlContent = new FormUrlEncodedContent(urlParams);
        var query = await urlContent.ReadAsStringAsync(cancellation);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/users?{query}");
        using var response = await _httpClient.SendAsync(request, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var applicationUsersPaginatedList = JsonSerializer.Deserialize<PaginatedDto<ApplicationUserDto>>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new PaginatedDto<ApplicationUserDto>();
        applicationUsersPaginatedList.Items ??= new List<ApplicationUserDto>();
        return applicationUsersPaginatedList;
    }

    public async Task<PaginatedDto<RecipeEntitlementDto>> GetRecipeEntitlementsAsync(string userId, int page, int limit, CancellationToken cancellation = default)
    {
        var urlParams = new List<KeyValuePair<string, string>>
        {
            new ("userId", userId),
            new ("page", page.ToString()),
            new ("limit", limit.ToString())
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

    public async Task<PaginatedDto<RecipeEntitlementDto>> GetRecipeEntitlementsAsync(Guid recipeId, int page, int limit, CancellationToken cancellation = default)
    {
        var urlParams = new List<KeyValuePair<string, string>>
        {
            new ("recipeId", recipeId.ToString()),
            new ("page", page.ToString()),
            new ("limit", limit.ToString())
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