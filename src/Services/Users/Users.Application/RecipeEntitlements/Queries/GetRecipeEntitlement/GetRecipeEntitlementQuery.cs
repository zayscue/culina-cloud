using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.Users.Application.Interfaces;
using CulinaCloud.Users.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.Users.Application.RecipeEntitlements.Queries.GetRecipeEntitlement
{
    public class GetRecipeEntitlementQuery : IRequest<GetRecipeEntitlementResponse>
    {
        public Guid Id { get; set; }
    }

    public class GetRecipeEntitlementQueryHandler : IRequestHandler<GetRecipeEntitlementQuery, GetRecipeEntitlementResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRecipeEntitlementQueryHandler(
            IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetRecipeEntitlementResponse> Handle(GetRecipeEntitlementQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.RecipeEntitlements
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(RecipeEntitlement), request.Id);
            }

            var response = _mapper.Map<GetRecipeEntitlementResponse>(entity);

            return response;
        }
    }
}