using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using CulinaCloud.CookBook.Domain.Entities;
using MediatR;

namespace CulinaCloud.CookBook.Application.RecipeSteps.Queries.GetRecipeStep
{
    public class GetRecipeStepQuery : IRequest<GetRecipeStepResponse>
    {
        public Guid RecipeId { get; set; }
        public int Order { get; set; }
    }

    public class GetRecipeStepQueryHandler : IRequestHandler<GetRecipeStepQuery, GetRecipeStepResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRecipeStepQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GetRecipeStepResponse> Handle(GetRecipeStepQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.RecipeSteps.FindAsync(request.RecipeId, request.Order);
            if (entity == null)
            {
                throw new NotFoundException(
                    nameof(RecipeStep),
                    JsonSerializer.Serialize(new { RecipeId = request.RecipeId, Order = request.Order })
                );
            }
            var response = _mapper.Map<GetRecipeStepResponse>(entity);
            return response;
        }
    }
}