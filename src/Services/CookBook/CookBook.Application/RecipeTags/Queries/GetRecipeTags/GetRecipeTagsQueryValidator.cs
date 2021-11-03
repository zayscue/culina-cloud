using FluentValidation;

namespace CulinaCloud.CookBook.Application.RecipeTags.Queries.GetRecipeTags
{
    public class GetRecipeTagsQueryValidator : AbstractValidator<GetRecipeTagsQuery>
    {
        public GetRecipeTagsQueryValidator()
        {
            RuleFor(q => q.RecipeId)
                .NotEmpty();
        }
    }
}