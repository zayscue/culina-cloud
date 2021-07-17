using System.Threading;
using System.Threading.Tasks;
using CulinaCloud.CookBook.Application.Common.Interfaces;

namespace CulinaCloud.CookBook.Infrastructure.Authentication
{
    public abstract class Auth0SecretsProvider : ISecretsProvider<(string ClientId, string ClientSecret)>
    {
        public abstract Task<(string ClientId, string ClientSecret)> GetSecrets(CancellationToken cancellationToken = default);
    }
}