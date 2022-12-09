using FluentValidation;

namespace CulinaCloud.Users.Application.ApplicationUsers.Queries.GetApplicationUser
{
    public class GetApplicationUserQueryValidator : AbstractValidator<GetApplicationUserQuery>
    {
        public GetApplicationUserQueryValidator()
        {
            RuleFor(q => q.Id)
                .NotEmpty();
        }
    }
}