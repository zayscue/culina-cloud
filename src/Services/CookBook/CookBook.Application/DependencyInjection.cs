using Microsoft.Extensions.DependencyInjection;

namespace Culina.CookBook.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services;
        }
    }
}
