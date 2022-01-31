namespace CulinaCloud.Web.BFF.APIGateway.Interfaces;

public interface IAnalyticsService
{
    Task<PaginatedListDto<Guid>> GetPersonalizedRecipeRecommendationsAsync(string userId,
        CancellationToken cancellation);
}