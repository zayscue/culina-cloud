using System.Security.Claims;
using Culina.CookBook.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Culina.CookBook.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private const string Anonymous = "Anonymous";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Anonymous;
    }
}
