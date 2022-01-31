﻿namespace CulinaCloud.Web.BFF.APIGateway.Interfaces;

public interface IUsersService
{
    Task<PaginatedListDto<Guid>?> GetUsersFavoritesAsync(string userId, CancellationToken cancellation = default);
    Task<PaginatedListDto<Guid>?> GetUsersFavoritesAsync(string userId, List<Guid> recipeIds,
        CancellationToken cancellation = default);
    Task<FavoriteDto?> CreateFavoriteAsync(FavoriteDto favorite, CancellationToken cancellation = default);
    Task DeleteFavoriteAsync(FavoriteDto favorite, CancellationToken cancellation = default);
}