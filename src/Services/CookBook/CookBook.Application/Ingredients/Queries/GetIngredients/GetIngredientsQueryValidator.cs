using FluentValidation;

namespace CulinaCloud.CookBook.Application.Ingredients.Queries.GetIngredients
{
    public class GetIngredientsQueryValidator : AbstractValidator<GetIngredientsQuery>
    {
        public GetIngredientsQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1).WithMessage("Page at least greater than or equal to 1.");

            RuleFor(x => x.Limit)
                .GreaterThanOrEqualTo(1).WithMessage("Limit at least greater than or equal to 1.");
        }
    }
}
