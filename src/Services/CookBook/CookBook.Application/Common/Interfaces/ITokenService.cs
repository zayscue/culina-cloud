using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.CookBook.Application.Common.Interfaces
{
    public interface ITokenService
    {
        Task<(string TokenType, string AccessToken)> GetToken(CancellationToken cancellationToken = default);
    }
}