using System;

namespace CulinaCloud.Users.Application.Exceptions
{
    public class RecipeHasNoAuthorException : Exception
    {
        public RecipeHasNoAuthorException(Guid recipeId) : base($"Recipe ${recipeId} has no author, and so no one is able to modify the recipe's entitlements.")
        {
        }
    }
}