namespace SSO.Application.Client.Queries.GetClientCreateViewModel
{
    using ApiResource.Queries.GetApiResourceList;
    using Commands.CreateClient;
    using IdentityResource.Queries.GetIdentityResourceList;
    using MediatR;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class ClientCreateViewModelQueryHandler : IRequestHandler<ClientCreateViewModelQuery, ClientCreateViewModel>
    {
        private readonly IMediator _mediator;

        public ClientCreateViewModelQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ClientCreateViewModel> Handle(ClientCreateViewModelQuery request, CancellationToken cancellationToken)
        {
            var viewModel = new ClientCreateViewModel
            {
                Command = request.Command ?? new CreateClientCommand(),
                ApiResources = (await _mediator.Send(new GetApiResourceListQuery())).Select((x) => x.Name),
                IdentityResources = (await _mediator.Send(new GetIdentityResourceListQuery())).Select((x) => x.Name)
            };

            return viewModel;
        }
    }
}
