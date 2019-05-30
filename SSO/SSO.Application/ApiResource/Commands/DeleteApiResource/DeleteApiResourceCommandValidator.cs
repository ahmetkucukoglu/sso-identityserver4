namespace SSO.Application.ApiResource.Commands.DeleteApiResource
{
    using FluentValidation;

    public class DeleteApiResourceCommandValidator : AbstractValidator<DeleteApiResourceCommand>
    {
        public DeleteApiResourceCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
