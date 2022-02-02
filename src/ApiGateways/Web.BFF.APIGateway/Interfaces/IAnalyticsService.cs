﻿namespace CulinaCloud.Web.BFF.APIGateway.Interfaces;

public interface IAnalyticsService
{
    Task<PaginatedDto<RecipeRecommendationDto>> GetPersonalizedRecipeRecommendationsAsync(string userId, int page, int limit,
        CancellationToken cancellation = default);

    Task<PaginatedDto<RecipeSimilarityDto>> GetSimilarRecipesAsync(Guid recipeId, int page, int limit,
        CancellationToken cancellation = default);
}