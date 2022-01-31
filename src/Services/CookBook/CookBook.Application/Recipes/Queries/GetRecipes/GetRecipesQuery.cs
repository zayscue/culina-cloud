using System;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.BuildingBlocks.Application.Common.Models;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.CookBook.Application.Recipes.Queries.GetRecipes
{
    public class GetRecipesQuery : IRequest<PaginatedList<GetRecipesResponse>>
    {
        public string Name { get; set; }
        public List<Guid> RecipeIds { get; set; } = new ();
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
    }

    public class GetRecipesQueryHandler : IRequestHandler<GetRecipesQuery, PaginatedList<GetRecipesResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRecipesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<GetRecipesResponse>> Handle(GetRecipesQuery request,
            CancellationToken cancellationToken)
        {
            var query = _context.Recipes
                .AsNoTracking()
                .Include(x => x.Images)
                .ThenInclude(x => x.Image)
                .AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                query = query.Where(r => EF.Functions.Like(r.Name.ToLower(), $"%{request.Name.Trim().ToLower()}%"));
            }

            if (request.RecipeIds.Count > 0)
            {
                query = query.Where(r => request.RecipeIds.Contains(r.Id));
            }
            
            var response = await query
                .ProjectTo<GetRecipesResponse>(_mapper.ConfigurationProvider)
                .ToPaginatedListAsync(request.Page, request.Limit);

            return response;
        }
    }
}