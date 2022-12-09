using System;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.Users.Domain.Entities;

namespace CulinaCloud.Users.Application.Favorites.Queries.GetFavorites
{
    public class GetFavoritesResponse : IMapFrom<Favorite>
    {
        public Guid RecipeId {  get; set; }
        public string UserId { get; set; }
    }
}
