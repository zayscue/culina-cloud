using AutoMapper;
using CulinaCloud.Analytics.Application.Interfaces;
using CulinaCloud.Analytics.Domain.Entities;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.BuildingBlocks.Common.Interfaces;
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
        public DateTime? Submitted { get; set; } = null;
        public int RatingCount { get; set; } = 0;
        public int RatingSum { get; set; } = 0;
        public decimal? RatingAverage { get; set; } = null;
        public decimal? RatingWeightedAverage { get; set; } = null;
        public string CreatedBy { get; set; }
    }

    public class CreateRecipePopularityCommandHandler : IRequestHandler<CreateRecipePopularityCommand, CreateRecipePopularityResponse>
    {

        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly IMapper _mapper;

        public CreateRecipePopularityCommandHandler(IApplicationDbContext context, IDateTime dateTime, IMapper mapper)
        {
            _context = context;
            _dateTime = dateTime;
            _mapper = mapper;
        }

        public async Task<CreateRecipePopularityResponse> Handle(CreateRecipePopularityCommand request, CancellationToken cancellationToken)
        {
            var date = DateOnly.FromDateTime(_dateTime.Now);
            var entity = new RecipePopularity
            {
                RecipeId = request.RecipeId,
                Submitted = request.Submitted.HasValue ? DateOnly.FromDateTime(request.Submitted.Value) : date,
                RatingCount = request.RatingCount,
                RatingSum = request.RatingSum,
                RatingAverage = request.RatingAverage.HasValue ? request.RatingAverage.Value : 0,
                RatingWeightedAverage = request.RatingWeightedAverage.HasValue ? request.RatingWeightedAverage.Value : 0,
                CreatedBy = request.CreatedBy
            };

            var count = (decimal)request.RatingCount;
            var sum = (decimal)request.RatingSum;
            var average = (decimal)0.00;
            if (count > 0 && sum > 0)
            {
                average = sum / count;
            }
            else
            {
                count = average;
                sum = average;
            }
            if (!request.RatingAverage.HasValue)
            {
                entity.RatingAverage = average;
                entity.RatingCount = (int)count;
                entity.RatingSum = (int)sum;
            }

            try
            {
                if (!request.RatingWeightedAverage.HasValue)
                {
                    await _context.Database.ExecuteSqlInterpolatedAsync(@$"
                        select ""Analytics"".""CreateRecipePopularity"" (
                            {entity.RecipeId},
                            {entity.Submitted},
                            {entity.RatingCount},
                            {entity.RatingSum},
                            {entity.RatingAverage},
                            {entity.CreatedBy}
                        );
                    ");
                    entity = await _context.RecipePopularity
                        .AsNoTracking()
                        .SingleOrDefaultAsync(x => x.RecipeId == entity.RecipeId);
                }
                else
                {
                    await _context.RecipePopularity.AddAsync(entity, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                }

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
