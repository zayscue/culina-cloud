using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CulinaCloud.Analytics.Application.Models;

namespace CulinaCloud.Analytics.Application.Interfaces
{
    public interface IRecommendationService
    {
        Task<IEnumerable<RecommendationResult>> GetPersonalRecipeRecommendationsAsync(string userId,
            CancellationToken cancellationToken = default);
    }
}
