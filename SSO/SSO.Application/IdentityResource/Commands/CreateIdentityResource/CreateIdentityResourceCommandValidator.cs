namespace SSO.Application.IdentityResource.Commands.CreateIdentityResource
{
    using FluentValidation;

    public class CreateIdentityResourceCommandValidator : AbstractValidator<CreateIdentityResourceCommand>
    {
        public CreateIdentityResourceCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.DisplayName).NotEmpty();
        }
    }
}
