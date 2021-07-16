using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace CulinaCloud.BuildingBlocks.Authorization.HasScope
{
    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "scope"))
            {
                return Task.CompletedTask;
            }
            var scopes = context.User.FindFirst("scope").Value.Split(" ");
            if (scopes.Any(s => string.Equals(s, requirement.Scope, System.StringComparison.OrdinalIgnoreCase)))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
