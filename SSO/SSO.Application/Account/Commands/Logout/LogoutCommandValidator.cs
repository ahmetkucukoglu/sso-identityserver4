namespace SSO.Application.Account.Commands.Logout
{
    using FluentValidation;

    public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator() { }
    }
}
