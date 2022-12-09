using System;

namespace CulinaCloud.Users.Application.Exceptions
{

    public class NoEntitlementException : Exception
    {
        public NoEntitlementException(Guid recipeId) : base($"Can't Modify Entitlements to Recipe {recipeId} If User is Has No Entitlement to Said Recipe.")
        {
        }
    }
}
