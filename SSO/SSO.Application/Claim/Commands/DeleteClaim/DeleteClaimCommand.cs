namespace SSO.Application.Claim.Commands.DeleteClaim
{
    using MediatR;

    public class DeleteClaimCommand : IRequest
    {
        public string Type { get; set; }
    }
}
