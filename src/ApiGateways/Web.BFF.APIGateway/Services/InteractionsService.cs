using System.Text;

namespace CulinaCloud.Web.BFF.APIGateway.Services;

public class InteractionsService : IInteractionsService
{
    private const string ServiceName = "Interactions";
    private readonly HttpClient _httpClient;

    public InteractionsService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
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
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/interactions/reviews?{query}");
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
        using var response = await _httpClient.PostAsync("/interactions/reviews", requestContent, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                throw new ReviewAlreadyExistsException(review.RecipeId, review.UserId ?? string.Empty);
            }
            else
            {
                throw new InternalServiceException(responseContent);
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