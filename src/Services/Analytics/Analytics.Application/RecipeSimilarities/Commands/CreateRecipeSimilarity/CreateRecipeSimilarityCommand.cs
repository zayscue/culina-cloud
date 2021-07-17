using MediatR;
using System;
using CulinaCloud.Analytics.Domain.Entities;
using System.Threading.Tasks;
using System.Threading;
using CulinaCloud.Analytics.Application.Interfaces;
using AutoMapper;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;

namespace CulinaCloud.Analytics.Application.RecipeSimilarities.Commands.CreateRecipeSimilarity
{
    public class CreateRecipeSimilarityCommand : IRequest<CreateRecipeSimilarityResponse>
    {
        public Guid RecipeId { get; set; }
        public Guid SimilarRecipeId { get; set; }
        public string SimilarityType { get; set; }
        public decimal SimilarityScore { get; set; }
    }

    public class CreateRecipeSimilarityCommandHandler : IRequestHandler<CreateRecipeSimilarityCommand, CreateRecipeSimilarityResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateRecipeSimilarityCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CreateRecipeSimilarityResponse> Handle(CreateRecipeSimilarityCommand request, CancellationToken cancellationToken)
        {
            var entity = new RecipeSimilarity
            {
                RecipeId = request.RecipeId,
                SimilarRecipeId = request.SimilarRecipeId,
                SimilarityType = request.SimilarityType,
                SimilarityScore = request.SimilarityScore
            };

            try
            {

                await _context.RecipeSimilarity.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var response = _mapper.Map<CreateRecipeSimilarityResponse>(entity);

                return response;
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException?.Message.Contains("PK_RecipeSimilarity", StringComparison.Ordinal) ??
                    false)
                {
                    throw new EntityConflictException(nameof(RecipeSimilarity), JsonSerializer.Serialize(new
                    {
                        RecipeId = entity.RecipeId.ToString(),
                        SimilarRecipeId = entity.SimilarRecipeId.ToString(),
                        SimilarityType = entity.SimilarityType
                    }));
                }

                throw;
            }
        }
    }
}
