namespace SSO.Application.Role.Commands.DeleteRole
{
    using FluentValidation;

    public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
