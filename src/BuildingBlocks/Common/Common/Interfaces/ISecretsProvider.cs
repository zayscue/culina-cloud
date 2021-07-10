using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.BuildingBlocks.Common.Interfaces
{
    public interface ISecretsProvider<T>
    {
        Task<T> GetSecrets(CancellationToken cancellationToken = default);
    }
}
