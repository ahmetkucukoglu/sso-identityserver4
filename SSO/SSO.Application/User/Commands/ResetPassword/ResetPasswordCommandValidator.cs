namespace SSO.Application.User.Commands.ResetPassword
{
    using FluentValidation;

    class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty();
        }
    }
}
