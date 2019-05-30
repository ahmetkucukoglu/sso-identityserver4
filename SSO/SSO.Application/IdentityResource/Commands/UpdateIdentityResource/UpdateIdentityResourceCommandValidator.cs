namespace SSO.Application.IdentityResource.Commands.UpdateIdentityResource
{
    using FluentValidation;

    public class UpdateIdentityResourceCommandValidator : AbstractValidator<UpdateIdentityResourceCommand>
    {
        public UpdateIdentityResourceCommandValidator()
        {
            RuleFor(x => x.DisplayName).NotEmpty();
        }
    }
}
