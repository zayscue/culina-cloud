using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.CookBook.Application.RecipeIngredients.Queries.GetRecipeIngredients
{
    public class GetRecipeIngredientsQuery : IRequest<List<GetRecipeIngredientsResponse>>
    {
        public Guid RecipeId { get; set; }
    }

    public class GetRecipeIngredientsQueryHandler : IRequestHandler<GetRecipeIngredientsQuery, List<GetRecipeIngredientsResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRecipeIngredientsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<GetRecipeIngredientsResponse>> Handle(GetRecipeIngredientsQuery request, CancellationToken cancellationToken)
        {
            var response = await _context.RecipeIngredients
                .AsNoTracking()
                .Include(x => x.Ingredient)
                .Where(x => x.RecipeId == request.RecipeId)
                .ProjectTo<GetRecipeIngredientsResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return response;
        }
    }
}