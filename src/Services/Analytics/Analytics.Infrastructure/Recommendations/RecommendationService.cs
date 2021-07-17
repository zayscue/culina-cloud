using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CulinaCloud.Analytics.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.Analytics.Infrastructure.Recommendations
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IApplicationDbContext _context;
        private readonly ICollaborativeFilteringModel _model;

        public RecommendationService(IApplicationDbContext context, ICollaborativeFilteringModel model)
        {
            _context = context;
            _model = model;
        }

        public async Task<IEnumerable<Guid>> GetPersonalRecipeRecommendationsAsync(string userId, CancellationToken cancellationToken = default)
        {
            var topTenThousandRecipes = await _context.RecipePopularity
                .AsNoTracking()
                .OrderByDescending(x => x.RatingWeightedAverage)
                .Take(10000)
                .ToListAsync(cancellationToken);

            return topTenThousandRecipes
                .Select(x => new
                {
                    x.RecipeId,
                    x.RatingWeightedAverage,
                    RecommendationScore = _model.PredictRecipeRecommendationScore(userId, x.RecipeId.ToString())
                })
                .OrderByDescending(x => x.RecommendationScore)
                .ThenByDescending(x => x.RatingWeightedAverage)
                .Select(x => x.RecipeId);
        }
    }
}
