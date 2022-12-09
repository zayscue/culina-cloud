using FluentValidation;

namespace CulinaCloud.Users.Application.Favorites.Commands.DeleteFavorite
{
    public class DeleteFavoriteCommandValidator : AbstractValidator<DeleteFavoriteCommand>
    {
        public DeleteFavoriteCommandValidator()
        {
            RuleFor(c => c.RecipeId)
                .NotEmpty();

            RuleFor(c => c.UserId)
                .NotEmpty();
        }
    }
}
