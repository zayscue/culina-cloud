using System.Security.Claims;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CulinaCloud.CookBook.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private const string Anonymous = "Anonymous";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId => (_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Anonymous)?.Replace("auth0|", "");
    }
}
