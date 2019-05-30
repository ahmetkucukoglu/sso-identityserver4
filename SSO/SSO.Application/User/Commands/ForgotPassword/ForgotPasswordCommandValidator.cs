namespace SSO.Application.User.Commands.ForgotPassword
{
    using FluentValidation;

    class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
        }
    }
}
