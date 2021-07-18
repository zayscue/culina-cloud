using FluentValidation;

namespace CulinaCloud.Users.Application.Favorites.Commands.CreateFavorite
{
    public class CreateFavoriteCommandValidator : AbstractValidator<CreateFavoriteCommand>
    {
        public CreateFavoriteCommandValidator()
        {
            RuleFor(c => c.RecipeId)
                .NotEmpty();
        }
    }
}
