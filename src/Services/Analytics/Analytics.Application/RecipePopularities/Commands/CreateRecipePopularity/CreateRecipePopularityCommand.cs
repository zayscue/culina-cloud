using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.Analytics.Application.RecipePopularities.Commands.CreateRecipePopularity
{
    public class CreateRecipePopularityCommand : IRequest<CreateRecipePopularityResponse>
    {
        public Guid RecipeId { get; set; }
        public string Submitted { get; set; }
        public int RatingCount { get; set; }
        public int RatingSum { get; set; }
        public decimal RatingAverage { get; set; }
        public decimal RatingWeightedAverage { get; set; }
    }

    public class CreateRecipePopularityCommandHandler : IRequestHandler<CreateRecipePopularityCommand, CreateRecipePopularityResponse>
    {
        public Task<CreateRecipePopularityResponse> Handle(CreateRecipePopularityCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
