using AutoMapper;
using AutoMapper.QueryableExtensions;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.BuildingBlocks.Application.Common.Models;
using CulinaCloud.BuildingBlocks.CurrentUser.Abstractions;
using CulinaCloud.Users.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.Users.Application.Favorites.Queries.GetFavorites
{
    public class GetFavoritesQuery : IRequest<PaginatedList<Guid>>
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 1000;
    }

    public class GetFavoritesQueryHandler : IRequestHandler<GetFavoritesQuery, PaginatedList<Guid>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetFavoritesQueryHandler(
            ICurrentUserService currentUserService,
            IApplicationDbContext context,
            IMapper mapper)
        {
            _currentUserService = currentUserService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<Guid>> Handle(GetFavoritesQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var response = await _context.Favorites
                .AsNoTracking()
                .OrderBy(x => x.Created)
                .Where(x => x.UserId == userId)
                .Select(x => x.RecipeId)
                .ProjectTo<Guid>(_mapper.ConfigurationProvider)
                .ToPaginatedListAsync(request.Page, request.Limit);
            return response;
        }
    }
}
