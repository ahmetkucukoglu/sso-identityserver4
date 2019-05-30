namespace SSO.Application.User.Commands.UpdateUser
{
    using System.Linq;
    using FluentValidation;

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.GivenName).NotEmpty();
            RuleFor(x => x.FamilyName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.SelectedRoles)
                .Must((x) => { return x?.Count() > 0; })
                .WithMessage("You must pick at least one role");
        }
    }
}
