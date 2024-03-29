﻿namespace CulinaCloud.Web.BFF.APIGateway.Interfaces;

public interface ICookBookService
{
    Task<RecipeDto> GetRecipeAsync(Guid id, CancellationToken cancellation = default);

    Task<RecipeDto> CreateRecipeAsync(CreateRecipeDto recipe, CancellationToken cancellation = default);

    Task<PaginatedDto<RecipesDto>> GetRecipesAsync(List<Guid> recipeIds, int page, int limit,
        CancellationToken cancellation = default);

    Task<PaginatedDto<RecipesDto>> GetRecipesAsync(string name, int page, int limit,
        CancellationToken cancellation = default);

    Task UpdateRecipeAsync(Guid recipeId, RecipeDto recipe,
        CancellationToken cancellation = default);

    Task<RecipeNutritionDto> CreateRecipeNutritionAsync(Guid recipeId, RecipeNutritionDto nutrition,
        CancellationToken cancellation = default);
    
    Task<RecipeNutritionDto> GetRecipeNutritionAsync(Guid recipeId, CancellationToken cancellation = default);

    Task UpdateRecipeNutritionAsync(Guid recipeId, RecipeNutritionDto recipeNutrition,
        CancellationToken cancellation = default);

    Task<List<RecipeStepDto>> GetRecipeStepsAsync(Guid recipeId, CancellationToken cancellation = default);

    Task<RecipeStepDto> GetRecipeStepAsync(Guid recipeId, int order, CancellationToken cancellation = default);

    Task BatchUpdateRecipeStepsAsync(Guid recipeId, List<RecipeStepDto> steps,
        CancellationToken cancellation = default);

    Task<RecipeImageDto> CreateRecipeImageAsync(Guid recipeId, RecipeImageDto image,
        CancellationToken cancellation = default);

    Task<List<RecipeImageDto>> GetRecipeImagesAsync(Guid recipeId, CancellationToken cancellation = default);

    Task BatchUpdateRecipeImagesAsync(Guid recipeId, List<RecipeImageDto> images,
        CancellationToken cancellation = default);

    Task<RecipeImageDto> GetRecipeImageAsync(Guid recipeId, Guid imageId, CancellationToken cancellation = default);

    Task<List<RecipeIngredientDto>> GetRecipeIngredientsAsync(Guid recipeId, CancellationToken cancellation = default);

    Task<RecipeIngredientDto> CreateRecipeIngredientAsync(Guid recipeId, RecipeIngredientDto recipeIngredient,
        CancellationToken cancellation = default);

    Task BatchUpdateRecipeIngredientsAsync(Guid recipeId, List<RecipeIngredientDto> ingredients,
        CancellationToken cancellation = default);

    Task<RecipeIngredientDto> GetRecipeIngredientAsync(Guid recipeId, Guid recipeIngredientId,
        CancellationToken cancellation = default);

    Task<List<RecipeTagDto>> GetRecipeTagsAsync(Guid recipeId, CancellationToken cancellation = default);

    Task<RecipeTagDto> CreateRecipeTagAsync(Guid recipeId, RecipeTagDto tag, CancellationToken cancellation = default);

    Task BatchUpdateRecipeTagsAsync(Guid recipeId, List<RecipeTagDto> tags,
        CancellationToken cancellation = default);

    Task<RecipeTagDto> GetRecipeTagAsync(Guid recipeId, Guid tagId, CancellationToken cancellation = default);


    Task<RecipeStatisticsDto> GetRecipeStatisticsAsync(CancellationToken cancellation = default);

    Task<PaginatedDto<IngredientDto>> GetIngredientsAsync(string name, int page, int limit,
        CancellationToken cancellation = default);

    Task<PaginatedDto<TagDto>> GetTagsAsync(string name, int page, int limit,
        CancellationToken cancellation = default);
}
