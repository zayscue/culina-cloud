﻿namespace CulinaCloud.Web.BFF.APIGateway.Interfaces;

public interface IAnalyticsService
{
    Task<PaginatedDto<RecipeRecommendationDto>> GetPersonalizedRecipeRecommendationsAsync(string userId, int page, int limit,
        CancellationToken cancellation = default);

    Task<PaginatedDto<RecipeSimilarityDto>> GetSimilarRecipesAsync(Guid recipeId, int page, int limit,
        CancellationToken cancellation = default);

    Task<PaginatedDto<RecipePopularityDto>> GetPopularRecipesAsync(int page, int limit, string orderBy, bool descending,
        CancellationToken cancellation = default);

    Task<RecipePopularityDto?> GetRecipePopularityAsync(Guid recipeId, CancellationToken cancellation = default);

    Task<PaginatedDto<RecipePopularityDto>> GetRecipePopularitiesAsync(List<Guid> recipeIds, CancellationToken cancellation = default);

    Task<PaginatedDto<RecentRecipeDto>> GetRecentRecipesAsync(int page, int limit, CancellationToken cancellation = default);

    Task<RecipePopularityDto> CreateRecipePopularityStatAsync(Guid recipeId, CancellationToken cancellation = default);

    Task<RecipePopularityDto> UpdateRecipePopularityStatAsync(Guid recipeId, int rating, CancellationToken cancellation = default);

    Task<RecipePopularityStatisticsDto> GetRecipePopularityStatisticsAsync(CancellationToken cancellationToken = default);
}