namespace CulinaCloud.Web.BFF.APIGateway.Interfaces;

public interface ICookBookService
{
    Task<RecipeDto> GetRecipeAsync(Guid id, CancellationToken cancellation = default);

    Task<PaginatedListDto<RecipesDto>> GetRecipesAsync(List<Guid> recipeIds, int page, int limit,
        CancellationToken cancellation = default);
}
