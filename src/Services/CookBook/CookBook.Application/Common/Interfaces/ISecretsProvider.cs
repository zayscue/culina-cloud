using System.Threading.Tasks;

namespace Culina.CookBook.Application.Common.Interfaces
{
    public interface ISecretsProvider<T>
    {
        Task<T> GetSecrets();
    }
}