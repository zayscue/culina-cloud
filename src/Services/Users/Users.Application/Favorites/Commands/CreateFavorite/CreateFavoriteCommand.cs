using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.Users.Application.Exceptions;
using CulinaCloud.Users.Application.Interfaces;
using CulinaCloud.Users.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.Users.Application.Favorites.Commands.CreateFavorite
{
    public class CreateFavoriteCommand : IRequest<CreateFavoriteResponse>
    {
        public string UserId { get; set; }
        public Guid RecipeId { get; set; }
        public string CreatedBy { get; set; }
    }

    public class CreateFavoriteCommandHandler : IRequestHandler<CreateFavoriteCommand, CreateFavoriteResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateFavoriteCommandHandler(
            IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CreateFavoriteResponse> Handle(CreateFavoriteCommand request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var recipeId = request.RecipeId;
            var createdBy = request.CreatedBy;
            var entity = new Favorite
            {
                UserId = userId,
                RecipeId = recipeId,
                CreatedBy = createdBy
            };
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
