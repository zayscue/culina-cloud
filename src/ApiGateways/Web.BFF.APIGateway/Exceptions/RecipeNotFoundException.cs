namespace CulinaCloud.Web.BFF.APIGateway.Exceptions;
public class RecipeNotFoundException : Exception
{
    public RecipeNotFoundException() : base("Recipe Not Found") {}
}
