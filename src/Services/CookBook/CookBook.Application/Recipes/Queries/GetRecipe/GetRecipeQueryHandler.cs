using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Culina.CookBook.Application.Common.Exceptions;
using Culina.CookBook.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Culina.CookBook.Application.Recipes.Queries.GetRecipe
{
    public class GetRecipeQueryHandler : IRequestHandler<GetRecipeQuery, GetRecipeResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        
        public GetRecipeQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<GetRecipeResponse> Handle(GetRecipeQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Recipes
                .Include(x => x.Steps)
                .Include(x => x.Ingredients)
                    .ThenInclude(x => x.Ingredient)
                .Include(x => x.Metadata)
                .Include(x => x.Images)
                    .ThenInclude(x => x.Image)
                .Include(x => x.Tags)
                    .ThenInclude(x => x.Tag)
                .Include(x => x.Nutrition)
                .Where(r => r.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Recipes), request.Id);
            }

            var response = _mapper.Map<GetRecipeResponse>(entity);

            return response;
        }
    }
}