using System;
using System.Linq;
using CulinaCloud.Users.Domain.Enums;
using FluentValidation;

namespace CulinaCloud.Users.Application.RecipeEntitlements.Queries.GetRecipeEntitlements
{
    public class GetRecipeEntitlementsQueryValidator : AbstractValidator<GetRecipeEntitlementsQuery>
    {
        private static readonly string[] OrderByOptions = { "granted", "grantedby", "userid", "recipeid", "type" };

        public GetRecipeEntitlementsQueryValidator()
        {
            RuleFor(q => q.Type)
                .Must(BeAValidRecipeEntitlementType);

            RuleFor(q => q.OrderBy)
                .NotEmpty()
                .Must(BeAValidOrderByOption);

            RuleFor(q => q.Limit)
                .LessThanOrEqualTo(1000);
        }

        private bool BeAValidRecipeEntitlementType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                return true;
            var validTypes = Enum.GetValues(typeof(RecipeEntitlementType))
                .Cast<RecipeEntitlementType>()
                .Select(v => v.ToString())
                .ToList();
            return validTypes.Contains(type.Trim().ToUpper());
        }

        private bool BeAValidOrderByOption(string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                return true;
            return OrderByOptions.Contains(orderBy.Trim().ToLower());
        }
    }
}