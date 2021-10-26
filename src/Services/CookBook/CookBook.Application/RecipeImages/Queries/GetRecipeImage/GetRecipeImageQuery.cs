using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using CulinaCloud.CookBook.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.CookBook.Application.RecipeImages.Queries.GetRecipeImage
{
    public class GetRecipeImageQuery : IRequest<GetRecipeImageResponse>
    {
        public Guid RecipeId { get; set; }
        public Guid ImageId { get; set; }
    }

    public class GetRecipeImageQueryHandler : IRequestHandler<GetRecipeImageQuery, GetRecipeImageResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRecipeImageQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GetRecipeImageResponse> Handle(GetRecipeImageQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.RecipeImages
                .AsNoTracking()
                .Include(x => x.Image)
                .SingleOrDefaultAsync(x => x.RecipeId == request.RecipeId
                    && x.ImageId == request.ImageId, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(
                    nameof(RecipeImage),
                    JsonSerializer.Serialize(new
                    {
                        RecipeId = request.RecipeId,
                        ImageId = request.ImageId
                    })
                );
            }

            var response = _mapper.Map<GetRecipeImageResponse>(entity);

            return response;
        }
    }
}