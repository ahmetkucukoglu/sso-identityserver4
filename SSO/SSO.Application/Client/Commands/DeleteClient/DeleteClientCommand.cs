namespace SSO.Application.Client.Commands.DeleteClient
{
    using MediatR;

    public class DeleteClientCommand : IRequest
    {
        public string Id { get; set; }
    }
}
