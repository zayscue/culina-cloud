using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace Culina.CookBook.Infrastructure.EventStore
{
    public class EventStoreAWSSecretsProvider : EventStoreSecretsProvider
    {
        private readonly IAmazonSecretsManager _secretsManager;

        public EventStoreAWSSecretsProvider(IAmazonSecretsManager secretsManager)
        {
            _secretsManager = secretsManager;
        }
        
        public override async Task<(string ClientId, string ClientSecret)> GetSecrets()
        {
            var secretResponse = await _secretsManager.GetSecretValueAsync(new GetSecretValueRequest
            {
                SecretId = "CulinaCloud/CookBookAPI/OAuthSecrets"
            });
            var secret = JsonSerializer.Deserialize<Dictionary<string, string>>(secretResponse.SecretString);
            return (ClientId: secret["clientId"], ClientSecret: secret["clientSecret"]);
        }
    }
}