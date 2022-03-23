using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.Users.Application.Interfaces;
using CulinaCloud.Users.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.Users.Application.Favorites.Commands.DeleteFavorite
{
    public class DeleteFavoriteCommand : IRequest<DeleteFavoriteResponse>
    {
        public Guid RecipeId { get; set; }
        public string UserId { get; set; }
    }

    public class DeleteFavoriteCommandHandler : IRequestHandler<DeleteFavoriteCommand, DeleteFavoriteResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteFavoriteCommandHandler(
            IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DeleteFavoriteResponse> Handle(DeleteFavoriteCommand request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var recipeId = request.RecipeId;
            var entity = await _context.Favorites
                .AsNoTracking()
                .SingleOrDefaultAsync(f => f.RecipeId == recipeId && f.UserId == userId, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Favorite), new { RecipeId = request.RecipeId, UserId = userId });
            }
            _context.Favorites.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<DeleteFavoriteResponse>(entity);
        }
    }
}
