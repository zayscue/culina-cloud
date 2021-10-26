using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using CulinaCloud.CookBook.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CulinaCloud.CookBook.Application.RecipeImages.Commands.CreateRecipeImage
{
    public class CreateRecipeImageCommand : IRequest<CreateRecipeImageResponse>
    {
        public Guid RecipeId { get; set; }
        public Guid? ImageId { get; set; }
        public string Url { get; set; }
        public string CreatedBy { get; set; }
    }

    public class CreateRecipeImageCommandHandler : IRequestHandler<CreateRecipeImageCommand, CreateRecipeImageResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateRecipeImageCommandHandler> _logger;

        public CreateRecipeImageCommandHandler(IApplicationDbContext context, IMapper mapper, ILogger<CreateRecipeImageCommandHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CreateRecipeImageResponse> Handle(CreateRecipeImageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var recipe = await _context.Recipes.FindAsync(request.RecipeId);
                if (recipe == null)
                {
                    throw new NotFoundException(nameof(Recipe), request.RecipeId);
                }
                var imageId = request.ImageId ?? Guid.NewGuid();
                var image = new Image
                {
                    Id = imageId,
                    CreatedBy = request.CreatedBy,
                    Url = request.Url
                };
                var entity = new RecipeImage
                {
                    RecipeId = request.RecipeId,
                    ImageId = imageId,
                    Recipe = recipe,
                    Image = image,
                    CreatedBy = request.CreatedBy
                };

                await _context.RecipeImages.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var response = _mapper.Map<CreateRecipeImageResponse>(entity);

                return response;
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException?.Message?.Contains("IX_Images_Url", StringComparison.Ordinal) ?? false)
                {
                    throw new EntityConflictException(nameof(RecipeImage), request.Url);
                }
                _logger.LogError(e.Message);
                throw e;
            }
        }
    }
}