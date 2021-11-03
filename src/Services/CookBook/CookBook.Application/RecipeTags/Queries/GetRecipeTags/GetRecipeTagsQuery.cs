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

namespace CulinaCloud.CookBook.Application.RecipeTags.Queries.GetRecipeTags
{
    public class GetRecipeTagsQuery : IRequest<List<GetRecipeTagsResponse>>
    {
        public Guid RecipeId { get; set; }
    }

    public class GetRecipeTagsQueryHandler : IRequestHandler<GetRecipeTagsQuery, List<GetRecipeTagsResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRecipeTagsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<GetRecipeTagsResponse>> Handle(GetRecipeTagsQuery request, CancellationToken cancellationToken)
        {
            var response = await _context.RecipeTags
                .AsNoTracking()
                .Include(x => x.Tag)
                .Where(x => x.RecipeId == request.RecipeId)
                .ProjectTo<GetRecipeTagsResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return response;
        }
    }
}