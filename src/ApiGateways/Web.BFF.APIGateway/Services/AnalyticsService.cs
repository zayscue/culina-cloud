namespace CulinaCloud.Web.BFF.APIGateway.Services;

public class AnalyticsService : IAnalyticsService
{
    public Task<PaginatedListDto<Guid>> GetPersonalizedRecipeRecommendationsAsync(string userId, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }
}