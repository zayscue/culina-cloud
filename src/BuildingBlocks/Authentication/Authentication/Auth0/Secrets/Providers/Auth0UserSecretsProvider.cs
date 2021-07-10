using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CulinaCloud.BuildingBlocks.Authentication.Auth0.Secrets.Providers
{
    public class Auth0UserSecretsProvider : Auth0SecretsProvider
    {
        private readonly IConfiguration _configuration;
        private readonly string _clientIdKey;
        private readonly string _clientSecretKey;

        public Auth0UserSecretsProvider(IConfiguration configuration, string clientIdKey, string clientSecretKey)
        {
            _configuration = configuration;
            _clientIdKey = clientIdKey;
            _clientSecretKey = clientSecretKey;
        }

        public override Task<Auth0Secrets> GetSecrets(CancellationToken cancellationToken = default)
        {
            var clientId = _configuration[_clientIdKey];
            var clientSecret = _configuration[_clientSecretKey];
            return Task.FromResult(new Auth0Secrets
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            });
        }
    }
}
