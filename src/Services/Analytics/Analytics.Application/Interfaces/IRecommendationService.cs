using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.Analytics.Application.Interfaces
{
    public interface IRecommendationService
    {
        Task<IQueryable<Guid>> GetPersonalRecipeRecommendationsAsync(string userId, CancellationToken cancellationToken = default);
    }
}
