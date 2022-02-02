namespace CulinaCloud.Web.BFF.APIGateway.Interfaces;

public interface IAnalyticsService
{
    Task<PaginatedListDto<RecipeRecommendationDto>> GetPersonalizedRecipeRecommendationsAsync(string userId, int page, int limit,
        CancellationToken cancellation = default);
}