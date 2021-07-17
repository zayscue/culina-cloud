using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CulinaCloud.CookBook.Infrastructure.Authentication
{
    public class Auth0UserSecretsProvider : Auth0SecretsProvider
    {
        private readonly IConfiguration _configuration;

        public Auth0UserSecretsProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public override Task<(string ClientId, string ClientSecret)> GetSecrets(CancellationToken cancellationToken = default)
        {
            var clientId = _configuration["ClientId"];
            var clientSecret = _configuration["ClientSecret"];
            return Task.FromResult((ClientId: clientId, ClientSecret: clientSecret));
        }
    }
}