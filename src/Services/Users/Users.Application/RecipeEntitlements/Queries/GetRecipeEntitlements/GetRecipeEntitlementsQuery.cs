using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.BuildingBlocks.Application.Common.Models;
using CulinaCloud.Users.Application.Interfaces;
using CulinaCloud.Users.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.Users.Application.RecipeEntitlements.Queries.GetRecipeEntitlements
{
    public class GetRecipeEntitlementsQuery : IRequest<PaginatedList<GetRecipeEntitlementsResponse>>
    {
        public Guid? RecipeId { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public string OrderBy { get; set; } = "granted";
        public bool Descending { get; set; } = true;
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 1000;
    }

    public class GetRecipeEntitlementsQueryHandler : IRequestHandler<GetRecipeEntitlementsQuery, PaginatedList<GetRecipeEntitlementsResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRecipeEntitlementsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<GetRecipeEntitlementsResponse>> Handle(GetRecipeEntitlementsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.RecipeEntitlements
                .AsNoTracking()
                .WhereIf(request.RecipeId.HasValue && request.RecipeId.Value != Guid.Empty,
                    x => x.RecipeId == request.RecipeId.Value)
                .WhereIf(!string.IsNullOrWhiteSpace(request.UserId),
                    x => x.UserId == request.UserId)
                .WhereIf(!string.IsNullOrWhiteSpace(request.Type),
                    x => x.Type == ((RecipeEntitlementType)Enum.Parse(typeof(RecipeEntitlementType),
                    request.Type.Trim().ToUpper())));

            var orderBy = request.OrderBy.Trim().ToLower();
            switch (orderBy)
            {
                case "granted":
                    query = request.Descending
                        ? query.OrderByDescending(x => x.Created)
                        : query.OrderBy(x => x.Created);
                    break;
                case "grantedby":
                    query = request.Descending
                        ? query.OrderByDescending(x => x.CreatedBy)
                        : query.OrderBy(x => x.CreatedBy);
                    break;
                case "userid":
                    query = request.Descending
                        ? query.OrderByDescending(x => x.UserId)
                        : query.OrderBy(x => x.UserId);
                    break;
                case "recipeid":
                    query = request.Descending
                        ? query.OrderByDescending(x => x.RecipeId)
                        : query.OrderBy(x => x.RecipeId);
                    break;
                case "type":
                    query = request.Descending
                        ? query.OrderByDescending(x => x.Type)
                        : query.OrderBy(x => x.Type);
                    break;
                default:
                    break;
            }

            return await query
                .ProjectTo<GetRecipeEntitlementsResponse>(_mapper.ConfigurationProvider)
                .ToPaginatedListAsync(request.Page, request.Limit);
        }
    }
}