using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CulinaCloud.Web.BFF.APIGateway.Services;

public class InteractionsService : IInteractionsService
{
    private const string ServiceName = "Interactions";
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenService;

    public InteractionsService(HttpClient httpClient, ITokenService tokenService)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }

    public async Task<PaginatedDto<ReviewDto>> GetRecipeReviews(Guid recipeId, int page, int limit, CancellationToken cancellation = default)
    {
        var urlParams = new List<KeyValuePair<string, string>>
        {
            new("recipeId", recipeId.ToString()),
            new("page", page.ToString()),
            new("limit", limit.ToString())
        };
        using var urlContent = new FormUrlEncodedContent(urlParams);
        var query = await urlContent.ReadAsStringAsync(cancellation);
        var token = await _tokenService.GetToken(cancellation);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/interactions/reviews?{query}")
        {
            Headers =
            {
                { HttpRequestHeader.Authorization.ToString(), $"{token.TokenType} {token.AccessToken}" }
            }
        };
        using var response = await _httpClient.SendAsync(request, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var recipeReviews = JsonSerializer.Deserialize<PaginatedDto<ReviewDto>>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new PaginatedDto<ReviewDto>();
        return recipeReviews;
    }

    public async Task<ReviewDto> CreateRecipeReviewAsync(ReviewDto review, CancellationToken cancellation = default)
    {
        var json = JsonSerializer.Serialize(review,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
        var token = await _tokenService.GetToken(cancellation);
        using var request = new HttpRequestMessage(HttpMethod.Post, "/interactions/reviews")
        {
            Content = requestContent,
            Headers =
            {
                { HttpRequestHeader.Authorization.ToString(), $"{token.TokenType} {token.AccessToken}" }
            }
        };
        using var response = await _httpClient.SendAsync(request, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                throw new ReviewAlreadyExistsException(review.RecipeId, review.UserId ?? string.Empty);
            }
            else
            {
                throw new InternalServiceException(ServiceName, response.StatusCode, responseContent);
            }
        }
        var createdReview = JsonSerializer.Deserialize<ReviewDto>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new ReviewDto();
        return createdReview;
    }
}