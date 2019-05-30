namespace SSO.Application.Client.Commands.CreateClient
{
    using FluentValidation;
    using UploadLogo;

    public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
    {
        public CreateClientCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.IdSuffix).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Secret).NotEmpty();
            RuleFor(x => x.AllowedGrantTypes).NotEmpty();
            RuleFor(x => x.Type).NotEmpty();
            RuleFor(x => x.PostLogoutRedirectUri).NotEmpty().When(x => x.Type == 1);
            RuleFor(x => x.RedirectUri).NotEmpty().When(x => x.Type == 1);
            RuleFor(x => x.LogoFile).NotEmpty().SetValidator(new LogoValidator());
        }
    }
}
