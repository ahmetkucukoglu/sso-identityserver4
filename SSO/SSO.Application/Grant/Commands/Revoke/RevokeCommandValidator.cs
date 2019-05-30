namespace SSO.Application.Grant.Commands.Revoke
{
    using FluentValidation;

    public class RevokeCommandValidator : AbstractValidator<RevokeCommand>
    {
        public RevokeCommandValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty();
        }
    }
}
