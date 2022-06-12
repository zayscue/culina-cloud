using AutoMapper;
using AutoMapper.QueryableExtensions;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.BuildingBlocks.Application.Common.Models;
using CulinaCloud.Users.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.Users.Application.ApplicationUsers.Queries.GetApplicationUsers
{
    public class GetApplicationUsersQuery : IRequest<PaginatedList<GetApplicationUsersResponse>>
    {
        public string Email { get; set; }
        public List<string> UserIds { get; set; } = new List<string>();
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 1000;
    }

    public class GetApplicationUsersQueryHandler : IRequestHandler<GetApplicationUsersQuery, PaginatedList<GetApplicationUsersResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetApplicationUsersQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<GetApplicationUsersResponse>> Handle(GetApplicationUsersQuery request, CancellationToken cancellationToken)
        {
            var query = _context.ApplicationUsers
                .AsNoTracking();
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                query = query.Where(x => x.Email.Trim().ToLower() == request.Email.Trim().ToLower());
            }
            if (request.UserIds != null && request.UserIds.Count > 0)
            {
                query = query.Where(x => request.UserIds.Contains(x.Id));
            }
            return await query
                .ProjectTo<GetApplicationUsersResponse>(_mapper.ConfigurationProvider)
                .ToPaginatedListAsync(request.Page, request.Limit);
        }
    }
}
