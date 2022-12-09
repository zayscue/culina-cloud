using AutoMapper;
using AutoMapper.QueryableExtensions;
using CulinaCloud.Analytics.Application.Interfaces;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.BuildingBlocks.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.Analytics.Application.RecipeSimilarities.Queries.GetRecipeSimilarities
{
    public class GetRecipeSimilaritiesQuery : IRequest<PaginatedList<GetRecipeSimilaritiesResponse>>
    {
        public Guid RecipeId { get; set; }
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 1000;
    }
    
    public class GetRecipeSimilaritiesQueryHandler : IRequestHandler<GetRecipeSimilaritiesQuery, PaginatedList<GetRecipeSimilaritiesResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRecipeSimilaritiesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<GetRecipeSimilaritiesResponse>> Handle(GetRecipeSimilaritiesQuery request, CancellationToken cancellationToken)
        {
            var response = await _context.RecipeSimilarity
                .Join(_context.RecipePopularity, 
                    similarity => similarity.SimilarRecipeId,
                    popularity => popularity.RecipeId,
                    (similarity, popularity) => new { Similarity = similarity, Popularity = popularity})
                .AsNoTracking()
                .OrderByDescending(x => x.Similarity.SimilarityScore)
                    .ThenByDescending(x => x.Popularity.RatingWeightedAverage)
                .Where(x => x.Similarity.RecipeId == request.RecipeId 
                            && x.Similarity.SimilarityType == "ingredient")
                .Select(x => new GetRecipeSimilaritiesResponse
                {
                    RecipeId = x.Similarity.RecipeId,
                    SimilarRecipeId = x.Similarity.SimilarRecipeId,
                    SimilarityType = x.Similarity.SimilarityType,
                    SimilarityScore = x.Similarity.SimilarityScore,
                    PopularityScore = x.Popularity.RatingWeightedAverage
                })
                .ToPaginatedListAsync(request.Page, request.Limit);
            return response;
        }
    }
}
