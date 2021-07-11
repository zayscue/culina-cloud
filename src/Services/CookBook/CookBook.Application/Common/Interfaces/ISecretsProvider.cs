using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.CookBook.Application.Common.Interfaces
{
    public interface ISecretsProvider<T>
    {
        Task<T> GetSecrets(CancellationToken cancellationToken = default);
    }
}