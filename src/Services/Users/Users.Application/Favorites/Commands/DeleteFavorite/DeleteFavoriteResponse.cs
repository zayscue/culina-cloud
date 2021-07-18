using System;

namespace CulinaCloud.Users.Application.Favorites.Commands.DeleteFavorite
{
    public class DeleteFavoriteResponse
    {
        public Guid RecipeId { get; set; }
        public string UserId { get; set; }
    }
}
