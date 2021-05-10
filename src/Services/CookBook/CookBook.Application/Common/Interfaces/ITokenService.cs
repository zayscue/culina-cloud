using System.Threading;
using System.Threading.Tasks;

namespace Culina.CookBook.Application.Common.Interfaces
{
    public interface ITokenService
    {
        Task<(string TokenType, string AccessToken)> GetToken(CancellationToken cancellationToken = default);
    }
}