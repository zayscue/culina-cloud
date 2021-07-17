using Microsoft.Extensions.Options;
using CulinaCloud.BuildingBlocks.Common.Interfaces;
using CulinaCloud.BuildingBlocks.Authentication.Abstractions;
using CulinaCloud.BuildingBlocks.Authentication.Auth0.Settings;
using CulinaCloud.BuildingBlocks.Authentication.Auth0.Secrets.Providers;

namespace CulinaCloud.BuildingBlocks.Authentication.Auth0
{
    public class Auth0TokenServiceManager : BaseTokenServiceManager
    {
        private readonly IDateTime _dateTime;
        private readonly IOptions<Auth0Settings> _settings;
        private readonly Auth0SecretsProvider _secretsProvider;

        public Auth0TokenServiceManager(
            IDateTime dateTime,
            IOptions<Auth0Settings> settings,
            Auth0SecretsProvider secretsProvider)
        {
            _dateTime = dateTime;
            _settings = settings;
            _secretsProvider = secretsProvider;
        }

        public override ITokenService GetTokenService(string audience)
        {
            while (true)
            {
                if (Services.TryGetValue(audience, out var service))
                {
                    if (service != null)
                    {
                        return service;
                    }
                }

                service = new Auth0TokenService(_dateTime, _settings, _secretsProvider, audience);
                if (Services.TryAdd(audience, service))
                {
                    return service;
                }
            }
        }
    }
}
