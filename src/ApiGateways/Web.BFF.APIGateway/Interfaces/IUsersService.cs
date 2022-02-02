namespace CulinaCloud.Web.BFF.APIGateway.Interfaces;

public interface IUsersService
{
    Task<PaginatedDto<Guid>> GetUsersFavoritesAsync(string userId, int page, int limit,
        CancellationToken cancellation = default);
    Task<PaginatedDto<Guid>> GetUsersFavoritesAsync(string userId, List<Guid> recipeIds, int page, int limit,
        CancellationToken cancellation = default);
    Task<FavoriteDto> CreateFavoriteAsync(FavoriteDto favorite, CancellationToken cancellation = default);
    Task DeleteFavoriteAsync(FavoriteDto favorite, CancellationToken cancellation = default);
}