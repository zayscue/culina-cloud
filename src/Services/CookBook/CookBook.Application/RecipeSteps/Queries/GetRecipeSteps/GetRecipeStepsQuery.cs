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

namespace CulinaCloud.CookBook.Application.RecipeSteps.Queries.GetRecipeSteps
{
    public class GetRecipeStepsQuery : IRequest<List<GetRecipeStepsResponse>>
    {
        public Guid RecipeId { get; set; }
    }

    public class GetRecipeStepsQueryHandler : IRequestHandler<GetRecipeStepsQuery, List<GetRecipeStepsResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRecipeStepsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<GetRecipeStepsResponse>> Handle(GetRecipeStepsQuery request, CancellationToken cancellationToken)
        {
            var response = await _context.RecipeSteps
                .AsNoTracking()
                .Where(x => x.RecipeId == request.RecipeId)
                .ProjectTo<GetRecipeStepsResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return response;
        }
    }
}