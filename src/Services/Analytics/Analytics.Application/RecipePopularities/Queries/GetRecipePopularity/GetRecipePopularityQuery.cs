using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.Analytics.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.Analytics.Application.RecipePopularities.Queries.GetRecipePopularity
{
    public class GetRecipePopularityQuery : IRequest<GetRecipePopularityResponse>
    {
        public Guid RecipeId { get; set; }
    }

    public class GetRecipePopularityQueryHandler : IRequestHandler<GetRecipePopularityQuery, GetRecipePopularityResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRecipePopularityQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GetRecipePopularityResponse> Handle(GetRecipePopularityQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.RecipePopularity
                .AsNoTracking()
                .Where(r => r.RecipeId == request.RecipeId)
                .SingleOrDefaultAsync(cancellationToken);
            var response = _mapper.Map<GetRecipePopularityResponse>(entity);
            return response;
        }
    }
}
