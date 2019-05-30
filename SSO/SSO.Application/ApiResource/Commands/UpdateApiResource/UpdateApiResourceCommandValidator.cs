namespace SSO.Application.ApiResource.Commands.UpdateApiResource
{
    using FluentValidation;

    public class UpdateApiResourceCommandValidator : AbstractValidator<UpdateApiResourceCommand>
    {
        public UpdateApiResourceCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.DisplayName).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
