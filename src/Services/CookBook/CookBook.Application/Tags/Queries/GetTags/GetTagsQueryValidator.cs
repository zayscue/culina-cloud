using FluentValidation;

namespace CulinaCloud.CookBook.Application.Tags.Queries.GetTags
{
    public class GetTagsQueryValidator : AbstractValidator<GetTagsQuery>
    {
        public GetTagsQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1).WithMessage("Page at least greater than or equal to 1.");

            RuleFor(x => x.Limit)
                .GreaterThanOrEqualTo(1).WithMessage("Limit at least greater than or equal to 1.");
        }
    }
}