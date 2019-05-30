namespace SSO.Application.Role.Commands.CreateRole
{
    using FluentValidation;

    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
