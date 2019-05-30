namespace SSO.Application.Claim.Commands.DeleteClaim
{
    using FluentValidation;

    public class DeleteClaimCommandValidator : AbstractValidator<DeleteClaimCommand>
    {
        public DeleteClaimCommandValidator()
        {
            RuleFor(x => x.Type).NotEmpty();
        }
    }
}
