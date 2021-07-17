using FluentValidation;

namespace CulinaCloud.CookBook.Application.Ingredients.Queries.GetIngredient
{
    public class GetIngredientQueryValidator : AbstractValidator<GetIngredientQuery>
    {
        public GetIngredientQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }
}