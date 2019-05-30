namespace SSO.Application.IdentityResource.Commands.DeleteIdentityResource
{
    using FluentValidation;

    public class DeleteIdentityResourceCommandValidator : AbstractValidator<DeleteIdentityResourceCommand>
    {
        public DeleteIdentityResourceCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
