namespace SSO.Application.Client.Commands.UploadLogo
{
    using FluentValidation;

    public class UploadLogoCommandValidator : AbstractValidator<UploadLogoCommand>
    {
        public UploadLogoCommandValidator()
        {
            RuleFor((x) => x.Id).NotEmpty();
            RuleFor(x => x.LogoFile).NotEmpty().SetValidator(new LogoValidator());
        }
    }
}
