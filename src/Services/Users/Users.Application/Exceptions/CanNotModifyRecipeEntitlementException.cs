using System;

namespace CulinaCloud.Users.Application.Exceptions
{
    public class CanNotModifyRecipeEntitlementException : Exception
    {
        public CanNotModifyRecipeEntitlementException(Guid recipeId, string type, string userId) : base($"User ${userId} is not priviledged enough to grant an entitlement of type {type} for recipe {recipeId}, so they are unable to modify the recipe's entitlements.")
        {
        }
    }
}