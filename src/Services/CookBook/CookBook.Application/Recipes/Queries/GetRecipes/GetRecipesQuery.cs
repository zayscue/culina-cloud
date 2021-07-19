using AutoMapper;
using AutoMapper.QueryableExtensions;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using CulinaCloud.CookBook.Application.Common.Mapping;
using CulinaCloud.CookBook.Application.Common.Models;
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

        public async Task<PaginatedList<GetRecipesResponse>> Handle(GetRecipesQuery request, CancellationToken cancellationToken)
        {
            var response = await _context.Recipes
                .AsNoTracking()
                .Include(x => x.Images)
                    .ThenInclude(x => x.Image)
                .Include(x => x.Nutrition)
                .Where(r => EF.Functions.Like(r.Name.ToLower(), $"%{request.Name.Trim().ToLower()}%"))
                .ProjectTo<GetRecipesResponse>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.Page, request.Limit);

            return response;
        }
    }
}
