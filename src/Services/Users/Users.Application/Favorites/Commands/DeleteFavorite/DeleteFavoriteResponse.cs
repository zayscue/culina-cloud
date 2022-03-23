using System;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.Users.Domain.Entities;

namespace CulinaCloud.Users.Application.Favorites.Commands.DeleteFavorite
{
    public class DeleteFavoriteResponse : IMapFrom<Favorite>
    {
        public Guid RecipeId { get; set; }
        public string UserId { get; set; }
    }
}
