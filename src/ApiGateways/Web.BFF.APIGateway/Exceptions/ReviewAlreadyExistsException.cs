namespace CulinaCloud.Web.BFF.APIGateway.Exceptions;
public class ReviewAlreadyExistsException : Exception
{
    public ReviewAlreadyExistsException(Guid recipeId, string userId) : base($"A Review By User {userId} Already Exists For Recipe {recipeId}") { }
}

