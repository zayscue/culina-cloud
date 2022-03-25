using System;
using System.Linq;
using CulinaCloud.Users.Domain.Enums;
using FluentValidation;

namespace CulinaCloud.Users.Application.RecipeEntitlements.Commands.CreateRecipeEntitlement
{
    public class CreateRecipeEntitlementCommandValidator : AbstractValidator<CreateRecipeEntitlementCommand>
    {
        public CreateRecipeEntitlementCommandValidator()
        {
            RuleFor(c => c.RecipeId)
                .NotEmpty();

            RuleFor(c => c.UserId)
                .NotEmpty()
                .MaximumLength(128);

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
            return validTypes.Contains(type.Trim().ToUpper());
        }
    }
}