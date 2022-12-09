using FluentValidation;

namespace CulinaCloud.CookBook.Application.RecipeTags.Queries.GetRecipeTag
{
    public class GetRecipeTagQueryValidator : AbstractValidator<GetRecipeTagQuery>
    {
        public GetRecipeTagQueryValidator()
        {
            RuleFor(q => q.RecipeId)
                .NotEmpty();
            RuleFor(q => q.TagId)
                .NotEmpty();
        }
    }
}