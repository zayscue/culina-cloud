namespace CulinaCloud.Web.BFF.APIGateway.Interfaces;

public interface IUsersService
{
    Task<PaginatedDto<FavoriteDto>> GetUsersFavoritesAsync(string userId, int page, int limit,
        CancellationToken cancellation = default);
    Task<PaginatedDto<FavoriteDto>> GetUsersFavoritesAsync(string userId, List<Guid> recipeIds, int page, int limit,
        CancellationToken cancellation = default);
    Task<FavoriteDto> CreateFavoriteAsync(FavoriteDto favorite, CancellationToken cancellation = default);
    Task DeleteFavoriteAsync(FavoriteDto favorite, CancellationToken cancellation = default);
    Task<ApplicationUserDto?> FindApplicationUserByEmailAsync(string email, CancellationToken cancellation = default);
    Task<ApplicationUserDto?> GetApplicationUserAsync(string userId, CancellationToken cancellation = default);
    Task<PaginatedDto<ApplicationUserDto>> GetApplicationUsersAsync(List<string> userIds, CancellationToken cancellation = default);
    Task<List<ApplicationUserPolicyDto>> GetApplicationUserPoliciesAsync(string userId, List<Guid>? recipeIds = null, CancellationToken cancellation = default);
    Task<PaginatedDto<RecipeEntitlementDto>> GetRecipeEntitlementsAsync(Guid recipeId, CancellationToken cancellation = default);
    Task<PaginatedDto<RecipeEntitlementDto>> GetRecipeEntitlementsAsync(Guid recipeId, int page, int limit, CancellationToken cancellation = default);
    Task<PaginatedDto<RecipeEntitlementDto>> GetRecipeEntitlementsAsync(string userId, int page, int limit, CancellationToken cancellation = default);
    Task<PaginatedDto<RecipeEntitlementDto>> GetRecipeEntitlementsAsync(Guid recipeId, string userId, CancellationToken cancellation = default);
    Task<PaginatedDto<RecipeEntitlementDto>> GetRecipeEntitlementsAsync(List<Guid> recipeIds, string userId, CancellationToken cancellation = default);
    Task<PaginatedDto<RecipeEntitlementDto>> GetRecipeEntitlementsAsync(Guid recipeId, List<string> userIds, CancellationToken cancellation = default);
    Task<RecipeEntitlementDto> CreateRecipeEntitlementAsync(RecipeEntitlementDto recipeEntitlement, CancellationToken cancellation = default);
    Task<bool> UpdateRecipeEntitlementAsync(Guid recipeEntitlementId, RecipeEntitlementDto recipeEntitlement, CancellationToken cancellation = default);
    Task<bool> DeleteRecipeEntitlementAsync(Guid recipeEntitlementId, RecipeEntitlementDto recipeEntitlement, CancellationToken cancellation = default);
    Task<UserStatisticsDto> GetUserStatisticsAsync(CancellationToken cancellationToken = default);
}