using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.BuildingBlocks.Authentication.Abstractions
{
    public interface ITokenService
    {
        Task<(string TokenType, string AccessToken)> GetToken(CancellationToken cancellationToken = default);
    }
}
