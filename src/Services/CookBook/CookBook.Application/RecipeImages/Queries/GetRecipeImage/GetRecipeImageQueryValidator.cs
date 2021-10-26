using FluentValidation;

namespace CulinaCloud.CookBook.Application.RecipeImages.Queries.GetRecipeImage
{
    public class GetRecipeImageQueryValidator : AbstractValidator<GetRecipeImageQuery>
    {
        public GetRecipeImageQueryValidator()
        {
            RuleFor(q => q.RecipeId)
                .NotEmpty();

            RuleFor(q => q.ImageId)
                .NotEmpty();
        }
    }
}