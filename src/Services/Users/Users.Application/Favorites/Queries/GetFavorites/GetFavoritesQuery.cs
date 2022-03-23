using AutoMapper;
using AutoMapper.QueryableExtensions;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.BuildingBlocks.Application.Common.Models;
using CulinaCloud.BuildingBlocks.CurrentUser.Abstractions;
using CulinaCloud.Users.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.Users.Application.Favorites.Queries.GetFavorites
{
    public class GetFavoritesQuery : IRequest<PaginatedList<GetFavoritesResponse>>
    {
        public string UserId { get; set; }
        public Guid? RecipeId { get; set; }
        public List<string> UserIds { get; set; } = new List<string>();
        public List<Guid> RecipeIds { get; set; } = new List<Guid>();
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 1000;
    }

    public class GetFavoritesQueryHandler : IRequestHandler<GetFavoritesQuery, PaginatedList<GetFavoritesResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetFavoritesQueryHandler(
            IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<GetFavoritesResponse>> Handle(GetFavoritesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Favorites
                .AsNoTracking()
                .OrderBy(x => x.Created)
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.UserId))
            {
                query = query.Where(x => x.UserId == request.UserId);
            }
            if (request.RecipeId.HasValue && request.RecipeId != Guid.Empty)
            {
                query = query.Where(x => x.RecipeId == request.RecipeId.Value);
            }
            if (request.UserIds != null && request.UserIds.Count > 0)
            {
                query = query.Where(x => request.UserIds.Contains(x.UserId));
            }
            if (request.UserIds != null && request.RecipeIds.Count > 0)
            {
                query = query.Where(x => request.RecipeIds.Contains(x.RecipeId));
            }
            var response = await query
                .ProjectTo<GetFavoritesResponse>(_mapper.ConfigurationProvider)
                .ToPaginatedListAsync(request.Page, request.Limit);
            return response;
        }
    }
}
