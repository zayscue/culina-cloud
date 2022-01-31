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
    
    public async Task<PaginatedListDto<Guid>?> GetUsersFavoritesAsync(string userId, CancellationToken cancellation = default)
    {
        using var urlContent = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>("userId", userId)
        });
        using var request = new HttpRequestMessage(HttpMethod.Get, "/users/favorites")
        {
            Content = urlContent
        };
        using var response = await _httpClient.SendAsync(request, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var favoritesPaginatedList = JsonSerializer.Deserialize<PaginatedListDto<Guid>>(responseContent, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        return favoritesPaginatedList;
    }

    public async Task<PaginatedListDto<Guid>?> GetUsersFavoritesAsync(string userId, List<Guid> recipeIds, CancellationToken cancellation = default)
    {
        var urlParams = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("userId", userId)
        };
        urlParams.AddRange(recipeIds.Select(recipeId => 
            new KeyValuePair<string, string>("recipeId", recipeId.ToString())));
        using var urlContent = new FormUrlEncodedContent(urlParams);
        using var request = new HttpRequestMessage(HttpMethod.Get, "/users/favorites")
        {
            Content = urlContent
        };
        using var response = await _httpClient.SendAsync(request, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var favoritesPaginatedList = JsonSerializer.Deserialize<PaginatedListDto<Guid>>(responseContent, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        return favoritesPaginatedList;
    }

    public async Task<FavoriteDto?> CreateFavoriteAsync(FavoriteDto favorite, CancellationToken cancellation = default)
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
        });
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
}