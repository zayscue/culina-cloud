using FluentValidation;

namespace CulinaCloud.Users.Application.ApplicationUsers.Commands.CreateApplicationUser
{
    public class CreateApplicationUserCommandValidator : AbstractValidator<CreateApplicationUserCommand>
    {
        public CreateApplicationUserCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .MaximumLength(128);

            RuleFor(c => c.DisplayName)
                .NotEmpty()
                .MaximumLength(32);

            RuleFor(c => c.Email)
                .NotEmpty()
                .MaximumLength(80);

            RuleFor(c => c.Picture)
                .NotEmpty()
                .MaximumLength(1024);

            RuleFor(c => c.CreatedBy)
                .NotEmpty()
                .MaximumLength(128);
        }
    }
}