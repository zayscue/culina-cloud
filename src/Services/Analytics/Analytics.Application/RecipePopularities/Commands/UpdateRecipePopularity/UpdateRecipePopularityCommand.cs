using AutoMapper;
using CulinaCloud.Analytics.Application.Interfaces;
using CulinaCloud.Analytics.Domain.Entities;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.Analytics.Application.RecipePopularities.Commands.UpdateRecipePopularity
{
    public class UpdateRecipePopularityCommand : IRequest<UpdateRecipePopularityResponse>
    {
        public Guid RecipeId { get; set; }
        public int Rating { get; set; } = 0;
        public string LastModifiedBy { get; set; }
    }

    public class UpdateRecipePopularityCommandHandler : IRequestHandler<UpdateRecipePopularityCommand, UpdateRecipePopularityResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateRecipePopularityCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UpdateRecipePopularityResponse> Handle(UpdateRecipePopularityCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.RecipePopularity
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.RecipeId == request.RecipeId);

            if (entity == null)
            {
                throw new NotFoundException(nameof(RecipePopularity), request.RecipeId);
            }

            await _context.Database.ExecuteSqlInterpolatedAsync(@$"
                        select ""Analytics"".""UpdateRecipePopularity"" (
                            {request.RecipeId},
                            {request.Rating},
                            {request.LastModifiedBy}
                        );
                    ");

            entity = await _context.RecipePopularity
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.RecipeId == entity.RecipeId);

            var response = _mapper.Map<UpdateRecipePopularityResponse>(entity);

            return response;
        }
    }
}
