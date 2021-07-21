using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CulinaCloud.BuildingBlocks.CurrentUser.Abstractions;

namespace CulinaCloud.BuildingBlocks.CurrentUser
{
    public class CurrentUserService : ICurrentUserService
    {
        public static readonly string Anonymous = "Anonymous";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId => (_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Anonymous)?.Replace("auth0|", "");
    }
}
