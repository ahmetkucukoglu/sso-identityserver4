namespace SSO.Application.ApiResource.Commands.CreateApiResource
{
    using FluentValidation;

    public class CreateApiResourceCommandValidator : AbstractValidator<CreateApiResourceCommand>
    {
        public CreateApiResourceCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.DisplayName).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
