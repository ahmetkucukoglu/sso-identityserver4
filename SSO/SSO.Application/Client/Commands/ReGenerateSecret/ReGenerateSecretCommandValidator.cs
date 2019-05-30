namespace SSO.Application.Client.Commands.ReGenerateSecret
{
    using FluentValidation;

    public class ReGenerateSecretCommandValidator : AbstractValidator<ReGenerateSecretCommand>
    {
        public ReGenerateSecretCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.NewSecret).NotEmpty();
        }
    }
}
