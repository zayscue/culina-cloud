using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using CulinaCloud.CookBook.Application.RecipeImages.Commands.CreateRecipeImage;
using CulinaCloud.CookBook.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.CookBook.Application.RecipeImages.Commands.BatchUpdateRecipeImages
{
    public class BatchUpdateRecipeImagesCommand : IRequest<List<CreateRecipeImageResponse>>
    {
        public Guid RecipeId { get; set; }
        public List<CreateRecipeImageCommand> Commands { get; set; }
    }

    public class BatchUpdateRecipeImagesCommandHandler :
        IRequestHandler<BatchUpdateRecipeImagesCommand, List<CreateRecipeImageResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BatchUpdateRecipeImagesCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<List<CreateRecipeImageResponse>> Handle(BatchUpdateRecipeImagesCommand request, CancellationToken cancellationToken)
        {
            var recipe = await _context.Recipes
                .AsNoTracking()
                .Include(x => x.Images)
                .SingleOrDefaultAsync(x => x.Id == request.RecipeId, cancellationToken);
            if (recipe == null)
            {
                throw new NotFoundException(nameof(Recipe), request.RecipeId);
            }
            
            var existingRecipeImages = recipe.Images.ToList();
            var commands = request.Commands;
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            _context.RecipeImages.RemoveRange(existingRecipeImages);
            await _context.SaveChangesAsync(cancellationToken);
            var recipeImages = new List<RecipeImage>();
            foreach (var command in commands)
            {
                var image = await _context.Images
                    .AsNoTracking().SingleOrDefaultAsync(x => x.Url.ToLower().Equals(command.Url.ToLower()),
                        cancellationToken);
                if (image == null)
                {
                    image = new Image
                    {
                        Id = command.ImageId == null || command.ImageId == Guid.Empty
                            ? Guid.NewGuid() 
                            : command.ImageId.Value,
                        Url = command.Url,
                        CreatedBy = command.CreatedBy
                    };
                    await _context.Images.AddAsync(image, cancellationToken);
                }
                var entity = new RecipeImage
                {
                    RecipeId = request.RecipeId,
                    CreatedBy = command.CreatedBy,
                    ImageId = image.Id
                };
                recipeImages.Add(entity);
            }
            await _context.RecipeImages.AddRangeAsync(recipeImages, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            var updatedRecipeImagesQuery = _context.RecipeImages
                .AsNoTracking()
                .Include(x => x.Image)
                .Where(x => x.RecipeId == request.RecipeId);

            var results = await updatedRecipeImagesQuery
                .ProjectToListAsync<CreateRecipeImageResponse>(_mapper.ConfigurationProvider, cancellationToken);

            return results;
        }
    }
}