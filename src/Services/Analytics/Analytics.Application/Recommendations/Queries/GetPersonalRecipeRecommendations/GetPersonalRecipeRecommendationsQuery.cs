using System;
using System.Threading;
using System.Threading.Tasks;
using CulinaCloud.Analytics.Application.Interfaces;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.BuildingBlocks.Application.Common.Models;
using CulinaCloud.BuildingBlocks.CurrentUser.Abstractions;
using MediatR;

namespace CulinaCloud.Analytics.Application.Recommendations.Queries.GetPersonalRecipeRecommendations
{
    public class GetPersonalRecipeRecommendationsQuery : IRequest<PaginatedList<Guid>>
    {
        public string UserId { get; set; }
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 1000;
    }

    public class GetPersonalRecipeRecommendationsQueryHandler : IRequestHandler<GetPersonalRecipeRecommendationsQuery,
        PaginatedList<Guid>>
    {
        private readonly IRecommendationService _recommendationService;

        public GetPersonalRecipeRecommendationsQueryHandler(IRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        public async Task<PaginatedList<Guid>> Handle(GetPersonalRecipeRecommendationsQuery request,
            CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var recommendations = await _recommendationService
                .GetPersonalRecipeRecommendationsAsync(userId, cancellationToken);
            return recommendations
                .ToPaginatedList(request.Page, request.Limit);
        }
    }
}