namespace SSO.Application.Client.Commands.DeleteClient
{
    using FluentValidation;

    public class DeleteClientCommandValidator : AbstractValidator<DeleteClientCommand>
    {
        public DeleteClientCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
