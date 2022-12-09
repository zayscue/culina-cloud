namespace CulinaCloud.Web.BFF.APIGateway.Interfaces;

public interface IInteractionsService
{
    Task<PaginatedDto<ReviewDto>> GetRecipeReviews(Guid recipeId, int page, int limit,
        CancellationToken cancellation = default);
    Task<ReviewDto> CreateRecipeReviewAsync(ReviewDto review, CancellationToken cancellation = default);
}