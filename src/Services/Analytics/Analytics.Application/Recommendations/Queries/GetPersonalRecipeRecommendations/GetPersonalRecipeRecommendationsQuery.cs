using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CulinaCloud.Analytics.Application.Interfaces;
using CulinaCloud.Analytics.Application.Models;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.BuildingBlocks.Application.Common.Models;
using CulinaCloud.BuildingBlocks.CurrentUser.Abstractions;
using MediatR;

namespace CulinaCloud.Analytics.Application.Recommendations.Queries.GetPersonalRecipeRecommendations
{
    public class GetPersonalRecipeRecommendationsQuery : IRequest<PaginatedList<PersonalRecipeRecommendationsResponse>>
    {
        public string UserId { get; set; }
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 1000;
    }

    public class GetPersonalRecipeRecommendationsQueryHandler : IRequestHandler<GetPersonalRecipeRecommendationsQuery,
        PaginatedList<PersonalRecipeRecommendationsResponse>>
    {
        private readonly IRecommendationService _recommendationService;
        private readonly IMapper _mapper;

        public GetPersonalRecipeRecommendationsQueryHandler(IRecommendationService recommendationService,
            IMapper mapper)
        {
            _recommendationService = recommendationService;
            _mapper = mapper;
        }

        public async Task<PaginatedList<PersonalRecipeRecommendationsResponse>> Handle(
            GetPersonalRecipeRecommendationsQuery request,
            CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var recommendations = await _recommendationService
                .GetPersonalRecipeRecommendationsAsync(userId, cancellationToken);
            return recommendations.AsQueryable()
                .ProjectTo<PersonalRecipeRecommendationsResponse>(_mapper.ConfigurationProvider)
                .ToPaginatedList(request.Page, request.Limit);
        }
    }
}