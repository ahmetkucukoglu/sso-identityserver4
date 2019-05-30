namespace SSO.Application.Consent.Commands.ProccessConsent
{
    using FluentValidation;

    public class ProccessConsentCommandValidator : AbstractValidator<ProccessConsentCommand>
    {
        public ProccessConsentCommandValidator()
        {
            RuleFor(x => x.ReturnUrl).NotEmpty();
            RuleFor(x => x.Action).NotEmpty().WithMessage("Invalid selection");
            RuleFor(x => x.ScopesConsented)
                .NotEmpty()
                .When(x => x.Action == "yes")
                .WithMessage("You must pick at least one permission");
        }
    }
}
