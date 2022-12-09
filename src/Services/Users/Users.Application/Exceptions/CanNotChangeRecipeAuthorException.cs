using System;

namespace CulinaCloud.Users.Application.Exceptions
{
    public class CanNotChangeRecipeAuthorException : Exception
    {
        public CanNotChangeRecipeAuthorException(Guid recipeId) : base($"Recipe {recipeId} can not have it's author changed.")
        {
        }
    }
}