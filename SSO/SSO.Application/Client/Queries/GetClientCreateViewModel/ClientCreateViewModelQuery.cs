namespace SSO.Application.Client.Queries.GetClientCreateViewModel
{
    using Commands.CreateClient;
    using MediatR;

    public class ClientCreateViewModelQuery : IRequest<ClientCreateViewModel>
    {
        public CreateClientCommand Command { get; set; }
    }
}
