using AutoMapper;
using AutoMapper.QueryableExtensions;
using CulinaCloud.Analytics.Application.Interfaces;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.BuildingBlocks.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.Analytics.Application.RecipePopularities.Queries.GetRecipePopularities
{
    public class GetRecipePopularitiesQuery : IRequest<PaginatedList<GetRecipePopularitiesResponse>>
    {
        public int Page { get; set; } = 1;
        public int? Limit { get; set; } = 10000;
        public string OrderBy { get; set; }
        public bool Descending { get; set; } = true;
    }

    public class GetRecipePopularitiesQueryHandler : IRequestHandler<GetRecipePopularitiesQuery, PaginatedList<GetRecipePopularitiesResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRecipePopularitiesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PaginatedList<GetRecipePopularitiesResponse>> Handle(GetRecipePopularitiesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.RecipePopularity
                .AsNoTracking()
                .AsQueryable();
            switch (request.OrderBy)
            {
                case "submitted":
                    query = request.Descending
                        ? query.OrderByDescending(x => x.Submitted)
                        : query.OrderBy(x => x.Submitted);
                    break;
                case "ratingCount":
                    query = request.Descending
                        ? query.OrderByDescending(x => x.RatingCount)
                        : query.OrderBy(x => x.RatingCount);
                    break;
                case "ratingSum":
                    query = request.Descending
                        ? query.OrderByDescending(x => x.RatingSum)
                        : query.OrderBy(x => x.RatingSum);
                    break;
                case "ratingAverage":
                    query = request.Descending
                        ? query.OrderByDescending(x => x.RatingAverage)
                        : query.OrderBy(x => x.RatingAverage);
                    break;
                case "ratingWeightedAverage":
                    query = request.Descending 
                        ? query.OrderByDescending(x => x.RatingWeightedAverage) 
                        : query.OrderBy(x => x.RatingWeightedAverage);
                    break;
                default:
                    break;
            }

            var response = await query
                .ProjectTo<GetRecipePopularitiesResponse>(_mapper.ConfigurationProvider)
                .ToPaginatedListAsync(request.Page, request.Limit);
            return response;
        }
    }
}
