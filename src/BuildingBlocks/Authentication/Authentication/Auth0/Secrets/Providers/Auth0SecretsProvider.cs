using System.Threading;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.Common.Interfaces;

namespace CulinaCloud.BuildingBlocks.Authentication.Auth0.Secrets.Providers
{
    public abstract class Auth0SecretsProvider : ISecretsProvider<Auth0Secrets>
    {
        public abstract Task<Auth0Secrets> GetSecrets(CancellationToken cancellationToken = default);
    }
}
