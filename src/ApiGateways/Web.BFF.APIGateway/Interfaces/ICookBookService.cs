namespace CulinaCloud.Web.BFF.APIGateway.Interfaces;

public interface ICookBookService
{
    Task<RecipeDto> GetRecipeAsync(Guid id, CancellationToken cancellation = default);

    Task<PaginatedDto<RecipesDto>> GetRecipesAsync(List<Guid> recipeIds, int page, int limit,
        CancellationToken cancellation = default);

    Task UpdateRecipeAsync(Guid recipeId, RecipeDto recipe,
        CancellationToken cancellation = default);

    Task UpdateRecipeNutritionAsync(Guid recipeId, RecipeNutritionDto recipeNutrition,
        CancellationToken cancellation = default);

    Task BatchUpdateRecipeStepsAsync(Guid recipeId, List<RecipeStepDto> steps,
        CancellationToken cancellation = default);

    Task BatchUpdateRecipeImagesAsync(Guid recipeId, List<RecipeImageDto> images,
        CancellationToken cancellation = default);
}
