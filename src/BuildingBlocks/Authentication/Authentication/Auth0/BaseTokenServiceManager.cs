using System.Collections.Concurrent;
using CulinaCloud.BuildingBlocks.Common.Interfaces;

namespace CulinaCloud.BuildingBlocks.Authentication.Auth0
{
    public abstract class BaseTokenServiceManager : ITokenServiceManager
    {
        protected readonly ConcurrentDictionary<string, ITokenService> Services =
            new ConcurrentDictionary<string, ITokenService>();
        public abstract ITokenService GetTokenService(string audience);
    }
}
