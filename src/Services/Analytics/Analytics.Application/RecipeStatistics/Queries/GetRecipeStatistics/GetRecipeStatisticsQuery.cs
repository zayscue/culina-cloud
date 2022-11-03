using AutoMapper;
using CulinaCloud.Analytics.Application.Interfaces;
using CulinaCloud.Analytics.Domain.Entities;
using CulinaCloud.BuildingBlocks.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.Analytics.Application.RecipeStatistics.Queries.GetRecipeStatistics
{
    public class GetRecipeStatisticsQuery : IRequest<GetRecipeStatisticsResponse> {}

    public class GetRecipeStatisticsQueryHandler : IRequestHandler<GetRecipeStatisticsQuery, GetRecipeStatisticsResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDateTime _dateTime;

        public GetRecipeStatisticsQueryHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));
        }

        public async Task<GetRecipeStatisticsResponse> Handle(GetRecipeStatisticsQuery request, CancellationToken cancellationToken)
        {
            var lastWeekCriteria = _dateTime.Now.AddDays(-7);
            var lastMonthCriteria = _dateTime.Now.AddDays(-30);
            var lastYearCriteria = _dateTime.Now.AddDays(-365);

            var numberOfRecipesCreatedInTheLastWeek = _context.RecipePopularity
                .AsNoTracking()
                .Count(x => x.Created >= lastWeekCriteria);
            var numberOfRecipesCreatedInTheLastMonth = _context.RecipePopularity
                .AsNoTracking()
                .Count(x => x.Created >= lastMonthCriteria);
            var numberOfRecipesCreatedInTheLastYear = _context.RecipePopularity
                .AsNoTracking()
                .Count(x => x.Created >= lastYearCriteria);

            var recipeCreationStatistics = new RecipeCreationStatistics
            {
                InTheLastWeek = numberOfRecipesCreatedInTheLastWeek,
                InTheLastMonth = numberOfRecipesCreatedInTheLastMonth,
                InTheLastYear = numberOfRecipesCreatedInTheLastYear
            };

            var popularRecipeStatistics = await _context.RecipePopularity
                .AsNoTracking()
                .OrderByDescending(x => x.RatingWeightedAverage)
                .Take(100)
                .Select(x => new RecipePopularityStatistic
                {
                    RatingWeightedAverage = x.RatingWeightedAverage,
                    RecipeId = x.RecipeId,
                    RatingAverage = x.RatingAverage,
                    RatingCount = x.RatingCount,
                    RatingSum = x.RatingSum
                })
                .ToListAsync(cancellationToken);

            var recentRecipeStatistics = await _context.RecipePopularity
                .AsNoTracking()
                .OrderByDescending(x => x.Created)
                .Take(100)
                .Select(x => new RecentRecipeStatistic
                {
                    RecipeId = x.RecipeId,
                    Created = x.Created
                })
                .ToListAsync(cancellationToken);

            var statistics = new Statistics
            {
                RecentRecipes = recentRecipeStatistics,
                MostPopularRecipes = popularRecipeStatistics,
                NumberOfRecipesCreated = recipeCreationStatistics
            };

            return _mapper.Map<GetRecipeStatisticsResponse>(statistics);
        }
    }
}
