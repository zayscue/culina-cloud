using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using CulinaCloud.CookBook.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.CookBook.Application.RecipeIngredients.Queries.GetRecipeIngredient
{
    public class GetRecipeIngredientQuery : IRequest<GetRecipeIngredientResponse>
    {
        public Guid RecipeId { get; set; }
        public Guid RecipeIngredientId { get; set; }
    }

    public class GetRecipeIngredientQueryHandler : IRequestHandler<GetRecipeIngredientQuery, GetRecipeIngredientResponse>
    {
         private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRecipeIngredientQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GetRecipeIngredientResponse> Handle(GetRecipeIngredientQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.RecipeIngredients
                .AsNoTracking()
                .Include(x => x.Ingredient)
                .SingleOrDefaultAsync(x => x.RecipeId == request.RecipeId
                    && x.Id == request.RecipeIngredientId, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(
                    nameof(RecipeIngredient),
                    JsonSerializer.Serialize(new
                    {
                        RecipeId = request.RecipeId,
                        RecipeIngredientId = request.RecipeIngredientId
                    })
                );
            }

            var response = _mapper.Map<GetRecipeIngredientResponse>(entity);

            return response;
        }
    }
}