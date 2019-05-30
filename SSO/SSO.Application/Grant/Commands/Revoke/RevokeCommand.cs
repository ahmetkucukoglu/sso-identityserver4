namespace SSO.Application.Grant.Commands.Revoke
{
    using MediatR;

    public class RevokeCommand : IRequest
    {
        public string ClientId { get; set; }
    }
}
