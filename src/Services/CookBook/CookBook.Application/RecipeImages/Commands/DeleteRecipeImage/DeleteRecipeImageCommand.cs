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

namespace CulinaCloud.CookBook.Application.RecipeImages.Commands.DeleteRecipeImage
{
    public class DeleteRecipeImageCommand : IRequest<DeleteRecipeImageResponse>
    {
        public Guid RecipeId { get; set; }
        public Guid ImageId { get; set; }
    }

    public class DeleteRecipeImageCommandHandler : IRequestHandler<DeleteRecipeImageCommand, DeleteRecipeImageResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteRecipeImageCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<DeleteRecipeImageResponse> Handle(DeleteRecipeImageCommand request, CancellationToken cancellationToken)
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
            _context.RecipeImages.Remove(entity);
            _context.Images.Remove(entity.Image);
            await _context.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<DeleteRecipeImageResponse>(entity);

            return response;
        }
    }
}