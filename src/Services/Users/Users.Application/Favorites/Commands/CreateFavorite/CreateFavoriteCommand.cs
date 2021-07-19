using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.BuildingBlocks.CurrentUser.Abstractions;
using CulinaCloud.Users.Application.Exceptions;
using CulinaCloud.Users.Application.Interfaces;
using CulinaCloud.Users.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.Users.Application.Favorites.Commands.CreateFavorite
{
    public class CreateFavoriteCommand : IRequest<CreateFavoriteResponse>
    {
        public Guid RecipeId { get; set; }
    }

    public class CreateFavoriteCommandHandler : IRequestHandler<CreateFavoriteCommand, CreateFavoriteResponse>
    {
        private readonly IRecipesService _recipesService;
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public CreateFavoriteCommandHandler(
            IRecipesService recipesService,
            IApplicationDbContext context,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _recipesService = recipesService;
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<CreateFavoriteResponse> Handle(CreateFavoriteCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var entity = new Favorite
            {
                UserId = userId,
                RecipeId = request.RecipeId
            };
            var recipeServiceIsHealthy = await _recipesService.CheckHealth(cancellationToken);
            if (!recipeServiceIsHealthy)
            {
                throw new RecipeServiceIsNotHealthyException();
            }
            var recipeExists = await _recipesService.RecipeExistsAsync(entity.RecipeId, cancellationToken);
            if (!recipeExists)
            {
                throw new RecipeDoesNotExistException(entity.RecipeId);
            }
            try
            {
                await _context.Favorites.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var response = _mapper.Map<CreateFavoriteResponse>(entity);

                return response;
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException?.Message.Contains("PK_Reviews", StringComparison.Ordinal) ??
                    false)
                {
                    throw new EntityConflictException(nameof(Favorite), JsonSerializer.Serialize(new { 
                        RecipeId = entity.RecipeId.ToString(),
                        UserId = entity.UserId
                    }));
                }

                throw;
            }
        }
    }
}
