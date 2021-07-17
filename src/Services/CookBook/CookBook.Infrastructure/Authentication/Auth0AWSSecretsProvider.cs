using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace CulinaCloud.CookBook.Infrastructure.Authentication
{
    public class Auth0AWSSecretsProvider : Auth0SecretsProvider
    {
        private readonly IAmazonSecretsManager _secretsManager;

        public Auth0AWSSecretsProvider(IAmazonSecretsManager secretsManager)
        {
            _secretsManager = secretsManager;
        }

        public override async Task<(string ClientId, string ClientSecret)> GetSecrets(CancellationToken cancellationToken = default)
        {
            var secretResponse = await _secretsManager.GetSecretValueAsync(new GetSecretValueRequest
            {
                SecretId = "CulinaCloud/CookBookAPI/OAuthSecrets"
            }, cancellationToken);

            var secret = JsonSerializer.Deserialize<Dictionary<string, string>>(secretResponse.SecretString);
            return (ClientId: secret["clientId"], ClientSecret: secret["clientSecret"]);
        }
    }
}