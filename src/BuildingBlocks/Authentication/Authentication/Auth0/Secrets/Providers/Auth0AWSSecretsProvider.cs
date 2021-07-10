using System.Text.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace CulinaCloud.BuildingBlocks.Authentication.Auth0.Secrets.Providers
{
    public class Auth0AWSSecretsProvider : Auth0SecretsProvider
    {
        private readonly IAmazonSecretsManager _secretsManager;
        private readonly string _secretId;

        public Auth0AWSSecretsProvider(IAmazonSecretsManager secretsManager, string secretId)
        {
            _secretsManager = secretsManager;
            _secretId = secretId;
        }

        public async override Task<Auth0Secrets> GetSecrets(CancellationToken cancellationToken = default)
        {
            var secretResponse = await _secretsManager.GetSecretValueAsync(new GetSecretValueRequest
            {
                SecretId = _secretId
            }, cancellationToken);

            var secret = JsonSerializer.Deserialize<Dictionary<string, string>>(secretResponse.SecretString);
            return new Auth0Secrets
            {
                ClientId = secret["clientId"],
                ClientSecret = secret["clientSecret"]
            };
        }
    }
}
