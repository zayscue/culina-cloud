namespace CulinaCloud.Web.BFF.APIGateway.Exceptions;
public class RecipeNotFoundException : Exception
{
    public RecipeNotFoundException(Guid recipeId) : base($"Recipe {recipeId} Not Found") {}
}
