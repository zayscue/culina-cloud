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

namespace CulinaCloud.CookBook.Application.RecipeImages.Commands.UpdateRecipeImage
{
    public class UpdateRecipeImageCommand : IRequest<UpdateRecipeImageResponse>
    {
        public Guid RecipeId { get; set; }
        public Guid ImageId { get; set; }
        public string Url { get; set; }
        public string LastModifiedBy { get; set; }
    }

    public class UpdateRecipeImageCommandHandler : IRequestHandler<UpdateRecipeImageCommand, UpdateRecipeImageResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateRecipeImageCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UpdateRecipeImageResponse> Handle(UpdateRecipeImageCommand request, CancellationToken cancellationToken)
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
            try
            {
                entity.Image.Url = request.Url;
                entity.Image.LastModifiedBy = request.LastModifiedBy;

                _context.Images.Update(entity.Image);
                await _context.SaveChangesAsync(cancellationToken);

                var response = _mapper.Map<UpdateRecipeImageResponse>(entity);

                return response;
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException?.Message?.Contains("IX_Images_Url", StringComparison.Ordinal) ?? false)
                {
                    throw new EntityConflictException(nameof(RecipeImage), request.Url);
                }
                throw e;
            }
        }
    }
}