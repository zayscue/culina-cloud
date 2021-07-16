using AutoMapper;
using CulinaCloud.Analytics.Application.Interfaces;
using CulinaCloud.Analytics.Domain.Entities;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateRecipePopularityCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CreateRecipePopularityResponse> Handle(CreateRecipePopularityCommand request, CancellationToken cancellationToken)
        {
            var entity = new RecipePopularity
            {
                RecipeId = request.RecipeId,
                Submitted = request.Submitted,
                RatingCount = request.RatingCount,
                RatingSum = request.RatingSum,
                RatingAverage = request.RatingAverage,
                RatingWeightedAverage = request.RatingWeightedAverage
            };

            try
            {

                await _context.RecipePopularity.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var response = _mapper.Map<CreateRecipePopularityResponse>(entity);

                return response;
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException?.Message.Contains("PK_RecipePopularity", StringComparison.Ordinal) ??
                    false)
                {
                    throw new EntityConflictException(nameof(RecipePopularity), entity.RecipeId.ToString());
                }

                throw;
            }
        }
    }
}
