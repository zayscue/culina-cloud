using AutoMapper;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using CulinaCloud.CookBook.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.Common.Interfaces;

namespace CulinaCloud.CookBook.Application.Statistics.Queries.GetStatistics
{ 
    public class GetStatisticsQuery : IRequest<GetStatisticsResponse>
    {

    }

    public class GetStatisticsQueryHandler : IRequestHandler<GetStatisticsQuery, GetStatisticsResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDateTime _dateTime;

        public GetStatisticsQueryHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));
        }

        public async Task<GetStatisticsResponse> Handle(GetStatisticsQuery request, CancellationToken cancellationToken)
        {
            var totalRecipes = await _context.Recipes.CountAsync(cancellationToken);
            var mostPopularTags = await _context.RecipeTags
                .AsNoTracking()
                .Include(x => x.Tag)
                .GroupBy(x => x.Tag.TagName)
                .Select(group => new TagPopularityRanking
                {
                    TagName = group.Key,
                    TotalRecipeTags = group.Count()
                })
                .Where(x => !string.IsNullOrWhiteSpace(x.TagName))
                .OrderByDescending(x => x.TotalRecipeTags)
                .Take(100)
                .ToListAsync(cancellationToken);
            var mostPopularIngredients = await _context.RecipeIngredients
                .AsNoTracking()
                .Include(x => x.Ingredient)
                .GroupBy(x => x.Ingredient.IngredientName)
                .Select(group => new IngredientPopularityRanking
                {
                    IngredientName = group.Key,
                    TotalIngredientReferences = group.Count()
                })
                .Where(x => !string.IsNullOrWhiteSpace(x.IngredientName))
                .OrderByDescending(x => x.TotalIngredientReferences)
                .Take(100)
                .ToListAsync(cancellationToken);

            var lastYear = _dateTime.Now.Date.AddDays(-365);
            var newRecipesInTheLastYear = await _context.Recipes
                .AsNoTracking()
                .Where(x => x.Created >= lastYear)
                .GroupBy(x => x.Created.Date)
                .Select(group => new
                {
                    Date = group.Key,
                    Count = group.Count()
                })
                .ToDictionaryAsync(x => x.Date, x => x.Count, cancellationToken);

            var dailyRecipeStatistics = new List<DailyRecipeStatistics>();
            var current = _dateTime.Now.Date;
            while (current > lastYear)
            {
                var currentDateStatistics = new DailyRecipeStatistics
                {
                    Date = current,
                    NewRecipes = 0
                };
                if (newRecipesInTheLastYear.ContainsKey(currentDateStatistics.Date))
                {
                    currentDateStatistics.NewRecipes = newRecipesInTheLastYear[currentDateStatistics.Date];
                }
                dailyRecipeStatistics.Add(currentDateStatistics);
                current = current.AddDays(-1);
            }

            var statistics = new Domain.Entities.Statistics
            {
                TotalRecipes = totalRecipes,
                MostPopularTags = mostPopularTags,
                MostPopularIngredients = mostPopularIngredients,
                DailyRecipeStatistics = dailyRecipeStatistics
            };
            return _mapper.Map<GetStatisticsResponse>(statistics);
        }
    }
}
