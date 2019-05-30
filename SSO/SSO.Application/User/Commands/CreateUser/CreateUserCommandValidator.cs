namespace SSO.Application.User.Commands.CreateUser
{
    using FluentValidation;
    using System.Linq;

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.GivenName).NotEmpty();
            RuleFor(x => x.FamilyName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.SelectedRoles)
                .Must((x) => { return x?.Count() > 0; })
                .WithMessage("You must pick at least one role");
        }
    }
}
