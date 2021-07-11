using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.BuildingBlocks.Application.Common.Models;
using CulinaCloud.Interactions.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.Interactions.Application.Reviews.Queries.GetReviews
{
    public class GetReviewsQuery : IRequest<PaginatedList<GetReviewsResponse>>
    {
        public Guid RecipeId { get; set; }
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 1000;
    }

    public class GetReviewsQueryHandler : IRequestHandler<GetReviewsQuery, PaginatedList<GetReviewsResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetReviewsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<GetReviewsResponse>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
        {
            var response = await _context.Reviews
                .AsNoTracking()
                .OrderBy(x => x.Created)
                .Where(x => x.RecipeId == request.RecipeId)
                .ProjectTo<GetReviewsResponse>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.Page, request.Limit);
            return response;
        }
    }
}
