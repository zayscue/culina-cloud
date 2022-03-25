﻿using System.Text;

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
}