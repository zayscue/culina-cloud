using AutoMapper;
using CulinaCloud.Analytics.Application.Interfaces;
using CulinaCloud.Analytics.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.Analytics.Application.Statistics.Queries.GetStatistics
{
    public class GetStatisticsQuery : IRequest<GetStatisticsResponse> {}

    public class GetStatisticsQueryHandler : IRequestHandler<GetStatisticsQuery, GetStatisticsResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetStatisticsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GetStatisticsResponse> Handle(GetStatisticsQuery request, CancellationToken cancellationToken)
        {
            var totalHistoricalRecipes = await _context.RecipePopularity
                .AsNoTracking()
                .CountAsync(cancellationToken);
            var totalHistoricalReviews = await _context.RecipePopularity
                .AsNoTracking()
                .SumAsync(x => x.RatingCount, cancellationToken);
            var mostPopularRecipes = await _context.RecipePopularity
                .AsNoTracking()
                .OrderByDescending(x => x.RatingWeightedAverage)
                .Take(100)
                .Select(x => new RecipePopularityRanking
                {
                    RecipeId = x.RecipeId,
                    RatingCount = x.RatingCount,
                    RatingSum = x.RatingSum,
                    RatingAverage = x.RatingAverage,
                    RatingWeightedAverage = x.RatingWeightedAverage
                })
                .ToListAsync(cancellationToken);

            var statistics = new Domain.Entities.Statistics
            {
                TotalHistoricalRecipes = totalHistoricalRecipes,
                TotalHistoricalReviews = totalHistoricalReviews,
                MostPopularRecipes = mostPopularRecipes
            };

            return _mapper.Map<GetStatisticsResponse>(statistics);
        }
    }
}
