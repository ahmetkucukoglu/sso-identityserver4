namespace SSO.Application.User.Commands.ChangePassword
{
    using FluentValidation;

    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.OldPassword).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
