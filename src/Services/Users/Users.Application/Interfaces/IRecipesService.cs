using System;
using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.Users.Application.Interfaces
{
    public interface IRecipesService
    {
        Task<bool> CheckHealth(CancellationToken cancellationToken = default);
        Task<bool> RecipeExistsAsync(Guid recipeId, CancellationToken cancellationToken = default);
    }
}
