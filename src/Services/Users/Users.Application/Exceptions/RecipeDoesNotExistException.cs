using System;

namespace CulinaCloud.Users.Application.Exceptions
{
    public class RecipeDoesNotExistException : Exception
    {
        public RecipeDoesNotExistException(Guid recipeId) : base($"Recipe {recipeId} does not exist.")
        {
        }
    }
}
