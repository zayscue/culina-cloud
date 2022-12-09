using FluentValidation;

namespace CulinaCloud.CookBook.Application.RecipeImages.Queries.GetRecipeImages
{
    public class GetRecipeImagesQueryValidator : AbstractValidator<GetRecipeImagesQuery>
    {
        public GetRecipeImagesQueryValidator()
        {
            RuleFor(q => q.RecipeId)
                .NotEmpty();
        }
    }
}