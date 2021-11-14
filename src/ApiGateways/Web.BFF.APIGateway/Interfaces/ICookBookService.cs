namespace CulinaCloud.Web.BFF.APIGateway.Interfaces;

public interface ICookBookService
{
    Task<RecipeDto> GetRecipeAsync(Guid id, CancellationToken cancellation = default);
}
