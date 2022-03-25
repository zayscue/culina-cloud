using System;

namespace CulinaCloud.Users.Application.Exceptions
{
    public class RecipeAuthorEntitlementDoesNotMatch : Exception
    {
        public RecipeAuthorEntitlementDoesNotMatch(string userId, string grantedBy) : base($"The granting user (${grantedBy}) and the entitlement user (${userId}) have to match when creating a recipe author entitlement.")
        {
        }
    }
}