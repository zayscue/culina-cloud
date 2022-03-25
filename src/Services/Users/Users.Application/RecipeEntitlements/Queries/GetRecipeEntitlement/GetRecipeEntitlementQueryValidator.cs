using FluentValidation;

namespace CulinaCloud.Users.Application.RecipeEntitlements.Queries.GetRecipeEntitlement
{
    public class GetRecipeEntitlementQueryValidator : AbstractValidator<GetRecipeEntitlementQuery>
    {
        public GetRecipeEntitlementQueryValidator()
        {
            RuleFor(q => q.Id)
                .NotEmpty();
        }
    }
}