using System;

namespace CulinaCloud.Users.Application.Favorites.Commands.CreateFavorite
{
    public class CreateFavoriteResponse
    {
        public Guid RecipeId {  get; set; }
        public string UserId { get; set; }
    }
}
