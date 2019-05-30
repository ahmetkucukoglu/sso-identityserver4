namespace SSO.Application.Client.Queries.GetClientEditViewModel
{
    using Commands.UpdateClient;
    using MediatR;

    public class ClientEditViewModelQuery : IRequest<ClientEditViewModel>
    {
        public string Id { get; set; }
        public UpdateClientCommand Command { get; set; }
    }
}
