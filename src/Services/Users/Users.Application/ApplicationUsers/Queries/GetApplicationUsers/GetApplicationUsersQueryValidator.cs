using FluentValidation;

namespace CulinaCloud.Users.Application.ApplicationUsers.Queries.GetApplicationUsers
{
    public class GetApplicationUsersQueryValidator : AbstractValidator<GetApplicationUsersQuery>
    {
        public GetApplicationUsersQueryValidator()
        {
            When(q => q.UserIds == null || q.UserIds.Count <= 0, () =>
            {
                RuleFor(q => q.Email)
                    .NotEmpty();
            });
            When(q => string.IsNullOrWhiteSpace(q.Email), () =>
            {
                RuleFor(c => c.UserIds)
                    .NotNull()
                    .NotEmpty();
            });
        }
    }
}
