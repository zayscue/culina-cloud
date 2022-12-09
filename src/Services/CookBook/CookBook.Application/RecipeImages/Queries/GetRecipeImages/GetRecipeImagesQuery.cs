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

namespace CulinaCloud.CookBook.Application.RecipeImages.Queries.GetRecipeImages
{
    public class GetRecipeImagesQuery : IRequest<List<GetRecipeImagesResponse>>
    {
        public Guid RecipeId { get; set; }
    }

    public class GetRecipeImagesQueryHandler : IRequestHandler<GetRecipeImagesQuery, List<GetRecipeImagesResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRecipeImagesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<GetRecipeImagesResponse>> Handle(GetRecipeImagesQuery request, CancellationToken cancellationToken)
        {
            var response = await _context.RecipeImages
                .AsNoTracking()
                .Include(x => x.Image)
                .Where(x => x.RecipeId == request.RecipeId)
                .ProjectTo<GetRecipeImagesResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return response;
        }
    }
}