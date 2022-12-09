using FluentValidation;

namespace CulinaCloud.Users.Application.RecipeEntitlements.Commands.DeleteRecipeEntitlement
{
    public class DeleteRecipeEntitlementCommandValidator : AbstractValidator<DeleteRecipeEntitlementCommand>
    {
        public DeleteRecipeEntitlementCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty();
        }
    }
}