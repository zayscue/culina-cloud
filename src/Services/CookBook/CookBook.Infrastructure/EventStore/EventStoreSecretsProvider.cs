using System.Threading;
using System.Threading.Tasks;
using Culina.CookBook.Application.Common.Interfaces;

namespace Culina.CookBook.Infrastructure.EventStore
{
    public abstract class EventStoreSecretsProvider : ISecretsProvider<(string ClientId, string ClientSecret)>
    {
        public abstract Task<(string ClientId, string ClientSecret)> GetSecrets(CancellationToken cancellationToken = default);
    }
}