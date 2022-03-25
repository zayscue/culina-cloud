using System;

namespace CulinaCloud.Users.Application.Exceptions
{
    public class CanNotDeleteRecipeAuthorException : Exception
    {
        public CanNotDeleteRecipeAuthorException(Guid recipeId) : base($"Recipe {recipeId} can not have it's author deleted.")
        {
        }
    }
}