using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Culina.CookBook.Infrastructure.EventStore
{
    public class EventStoreUserSecretsProvider : EventStoreSecretsProvider
    {
        private readonly IConfiguration _config;

        public EventStoreUserSecretsProvider(IConfiguration config)
        {
            _config = config;
        }
        
        public override Task<(string ClientId, string ClientSecret)> GetSecrets(CancellationToken cancellationToken = default)
        {
            var clientId = _config["ClientId"];
            var clientSecret = _config["ClientSecret"];
            return Task.FromResult((ClientId: clientId, ClientSecret: clientSecret));
        }
    }
}