namespace SSO.Application.Client.Commands.UpdateClient
{
    using FluentValidation;

    public class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
    {
        public UpdateClientCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.AllowedGrantTypes).NotEmpty();
            RuleFor(x => x.Type).NotEmpty();
            RuleFor(x => x.PostLogoutRedirectUri).NotEmpty().When(x => x.Type == 1);
            RuleFor(x => x.RedirectUri).NotEmpty().When(x => x.Type == 1);
        }
    }
}
