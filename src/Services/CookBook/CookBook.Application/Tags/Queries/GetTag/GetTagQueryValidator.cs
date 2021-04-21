using FluentValidation;

namespace Culina.CookBook.Application.Tags.Queries.GetTag
{
    public class GetTagQueryValidator : AbstractValidator<GetTagQuery>
    {
        public GetTagQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }
}