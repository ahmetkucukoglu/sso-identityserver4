namespace SSO.Application.Claim.Commands.CreateClaim
{
    using MediatR;

    public class CreateClaimCommand : IRequest
    {
        public string Type { get; set; }
    }
}
