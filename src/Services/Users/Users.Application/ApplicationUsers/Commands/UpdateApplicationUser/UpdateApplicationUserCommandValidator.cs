using FluentValidation;

namespace CulinaCloud.Users.Application.ApplicationUsers.Commands.UpdateApplicationUser
{
    public class UpdateApplicationUserCommandValidator : AbstractValidator<UpdateApplicationUserCommand>
    {
        public UpdateApplicationUserCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .MaximumLength(128);

            RuleFor(c => c.DisplayName)
                .MaximumLength(32);

            RuleFor(c => c.Picture)
                .MaximumLength(1024);

            RuleFor(c => c.LastModifiedBy)
                .NotEmpty()
                .MaximumLength(128);
        }
    }
}