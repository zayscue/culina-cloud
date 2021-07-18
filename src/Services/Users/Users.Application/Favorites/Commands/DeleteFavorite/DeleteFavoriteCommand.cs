using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.BuildingBlocks.CurrentUser.Abstractions;
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
    }

    public class DeleteFavoriteCommandHandler : IRequestHandler<DeleteFavoriteCommand, DeleteFavoriteResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public DeleteFavoriteCommandHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<DeleteFavoriteResponse> Handle(DeleteFavoriteCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var entity = await _context.Favorites
                .AsNoTracking()
                .Where(f => f.RecipeId == f.RecipeId && f.UserId == userId)
                .SingleOrDefaultAsync(cancellationToken);

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
