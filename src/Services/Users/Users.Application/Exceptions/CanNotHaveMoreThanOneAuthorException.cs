using System;

namespace CulinaCloud.Users.Application.Exceptions
{
    public class CanNotHaveMoreThanOneAuthorException : Exception
    {
        public CanNotHaveMoreThanOneAuthorException(Guid recipeId) : base($"Recipe {recipeId} already has an author and can't have another one.")
        {
        }
    }
}