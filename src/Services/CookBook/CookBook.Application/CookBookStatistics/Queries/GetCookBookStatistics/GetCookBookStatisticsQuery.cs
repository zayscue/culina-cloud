using AutoMapper;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using CulinaCloud.CookBook.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.CookBook.Application.CookBookStatistics.Queries.GetCookBookStatistics
{ 
    public class GetCookBookStatisticsQuery : IRequest<GetCookBookStatisticsResponse>
    {

    }

    public class GetCookBookStatisticsQueryHandler : IRequestHandler<GetCookBookStatisticsQuery, GetCookBookStatisticsResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCookBookStatisticsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GetCookBookStatisticsResponse> Handle(GetCookBookStatisticsQuery request, CancellationToken cancellationToken)
        {
            var recipeCount = await _context.Recipes.CountAsync(cancellationToken);
            var topRecipeTags = await _context.RecipeTags
                .AsNoTracking()
                .Include(x => x.Tag)
                .GroupBy(x => x.Tag.TagName)
                .Select(group => new RecipeTagStatistic
                {
                    TagName = group.Key,
                    RecipeCount = group.Count()
                })
                .Where(x => !string.IsNullOrWhiteSpace(x.TagName))
                .OrderByDescending(x => x.RecipeCount)
                .Take(25)
                .ToListAsync(cancellationToken);
            var topRecipeIngredients = await _context.RecipeIngredients
                .AsNoTracking()
                .Include(x => x.Ingredient)
                .GroupBy(x => x.Ingredient.IngredientName)
                .Select(group => new RecipeIngredientStatistic
                {
                    IngredientName = group.Key,
                    RecipeCount = group.Count()
                })
                .Where(x => !string.IsNullOrWhiteSpace(x.IngredientName))
                .OrderByDescending(x => x.RecipeCount)
                .Take(25)
                .ToListAsync(cancellationToken);
            var statistics = new Statistics
            {
                RecipeStatistics = new RecipeStatistics
                {
                    RecipeCount = recipeCount
                },
                TagStatistics = topRecipeTags,
                IngredientStatistics = topRecipeIngredients
            };
            return _mapper.Map<GetCookBookStatisticsResponse>(statistics);
        }
    }
}
