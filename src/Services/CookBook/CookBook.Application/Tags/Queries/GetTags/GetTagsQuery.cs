using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.BuildingBlocks.Application.Common.Models;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.CookBook.Application.Tags.Queries.GetTags
{
    public class GetTagsQuery : IRequest<PaginatedList<GetTagsResponse>>
    {
        public string Name { get; set; }
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 1000;
    }

    public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, PaginatedList<GetTagsResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTagsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<GetTagsResponse>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Tags
                .AsNoTracking()
                .OrderBy(x => x.TagName)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                query = query.Where(x =>
                    EF.Functions.Like(x.TagName.ToLower(), $"%{request.Name.Trim().ToLower()}%"));
            }

            return await query
                .ProjectTo<GetTagsResponse>(_mapper.ConfigurationProvider)
                .ToPaginatedListAsync(request.Page, request.Limit);

        }
    }
}