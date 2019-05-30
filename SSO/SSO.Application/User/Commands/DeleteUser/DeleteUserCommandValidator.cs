namespace SSO.Application.User.Commands.DeleteUser
{
    using FluentValidation;

    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
