using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.Analytics.Application.Interfaces
{
    public interface IRecommendationService
    {
        Task<IEnumerable<Guid>> GetPersonalRecipeRecommendationsAsync(string userId, CancellationToken cancellationToken = default);
    }
}
