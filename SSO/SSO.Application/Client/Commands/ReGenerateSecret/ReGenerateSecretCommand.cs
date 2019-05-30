namespace SSO.Application.Client.Commands.ReGenerateSecret
{
    using MediatR;

    public class ReGenerateSecretCommand : IRequest
    {
        public string Id { get; set; }

        public string NewSecret { get; set; }
    }
}
