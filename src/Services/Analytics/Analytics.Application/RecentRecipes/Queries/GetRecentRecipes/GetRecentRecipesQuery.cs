using AutoMapper;
using AutoMapper.QueryableExtensions;
using CulinaCloud.Analytics.Application.Interfaces;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.BuildingBlocks.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.Analytics.Application.RecentRecipes.Queries.GetRecentRecipes
{
    public class GetRecentRecipesQuery : IRequest<PaginatedList<RecentRecipeResponse>>
    {
        public DateTime? Lower { get; set; } = null;
        public DateTime? Upper { get; set; } = null;
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 1000;
    }

    public class GetRecentRecipesQueryHandler : IRequestHandler<GetRecentRecipesQuery, PaginatedList<RecentRecipeResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRecentRecipesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<RecentRecipeResponse>> Handle(GetRecentRecipesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.RecipePopularity
                .AsNoTracking();

            if (request.Lower.HasValue)
            {
                query = query.Where(x => x.Submitted >= DateOnly.FromDateTime(request.Lower.Value));
            }

            if (request.Upper.HasValue)
            {
                query = query.Where(x => x.Submitted <= DateOnly.FromDateTime(request.Upper.Value));
            }

            var recentRecipes = await query
                .OrderByDescending(x => x.Submitted)
                    .ThenByDescending(x => x.RatingWeightedAverage)
                .ProjectTo<RecentRecipeResponse>(_mapper.ConfigurationProvider)
                .ToPaginatedListAsync(request.Page, request.Limit);
            return recentRecipes;
        }
    }
}
