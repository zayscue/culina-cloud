using FluentValidation;

namespace CulinaCloud.Users.Application.ApplicationUsers.Queries.GetApplicationUsers
{
    public class GetApplicationUsersQueryValidator : AbstractValidator<GetApplicationUsersQuery>
    {
        public GetApplicationUsersQueryValidator()
        {
            RuleFor(q => q.Email)
                .NotEmpty();
        }
    }
}
