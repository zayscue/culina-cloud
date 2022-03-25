using System;

namespace CulinaCloud.Users.Application.Exceptions
{
    public class CanNotModifyRecipeEntitlementException : Exception
    {
        public CanNotModifyRecipeEntitlementException(Guid recipeId, string userId) : base($"User ${userId} is not the author of recipe {recipeId}, so they are unable to modify the recipe's entitlements.")
        {
        }
    }
}