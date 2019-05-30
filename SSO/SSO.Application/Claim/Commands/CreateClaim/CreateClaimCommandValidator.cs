namespace SSO.Application.Claim.Commands.CreateClaim
{
    using FluentValidation;

    public class CreateClaimCommandValidator : AbstractValidator<CreateClaimCommand>
    {
        public CreateClaimCommandValidator()
        {
            RuleFor(x => x.Type).NotEmpty();
        }
    }
}
