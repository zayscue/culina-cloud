using System;
using System.Linq;
using CulinaCloud.Users.Domain.Enums;
using FluentValidation;

namespace CulinaCloud.Users.Application.RecipeEntitlements.Commands.UpdateRecipeEntitlement
{
    public class UpdateRecipeEntitlementCommandValidator : AbstractValidator<UpdateRecipeEntitlementCommand>
    {
        public UpdateRecipeEntitlementCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty();

            RuleFor(c => c.Type)
                .NotEmpty()
                .Must(BeAValidRecipeEntitlementType);

            RuleFor(c => c.GrantedBy)
                .NotEmpty()
                .MaximumLength(128);
        }

        private bool BeAValidRecipeEntitlementType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                return false;
            var validTypes = Enum.GetValues(typeof(RecipeEntitlementType))
                .Cast<RecipeEntitlementType>()
                .Select(v => v.ToString())
                .ToList();
            validTypes.Remove(RecipeEntitlementType.AUTHOR.ToString());
            return validTypes.Contains(type.Trim().ToUpper());
        }
    }
}
