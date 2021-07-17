using Microsoft.AspNetCore.Authorization;

namespace CulinaCloud.BuildingBlocks.Authorization.HasScope
{
    public class HasScopeRequirement : IAuthorizationRequirement
    {
        public string Scope { get; }

        public HasScopeRequirement(string scope)
        {
            Scope = scope;
        }
    }
}
