using System;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.Users.Domain.Entities;

namespace CulinaCloud.Users.Application.Favorites.Commands.CreateFavorite
{
    public class CreateFavoriteResponse : IMapFrom<Favorite>
    {
        public Guid RecipeId {  get; set; }
        public string UserId { get; set; }
    }
}
