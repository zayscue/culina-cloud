using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using CulinaCloud.CookBook.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.CookBook.Application.RecipeNutritionFacts.Queries.GetRecipeNutrition
{
    public class GetRecipeNutritionQuery : IRequest<GetRecipeNutritionResponse>
    {
        public Guid RecipeId { get; set; }
    }

    public class GetRecipeNutritionQueryHandler : IRequestHandler<GetRecipeNutritionQuery, GetRecipeNutritionResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRecipeNutritionQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GetRecipeNutritionResponse> Handle(GetRecipeNutritionQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.RecipeNutrition
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.RecipeId == request.RecipeId, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(RecipeNutrition), request.RecipeId);
            }

            var resposne = _mapper.Map<GetRecipeNutritionResponse>(entity);

            return resposne;
        }
    }
}