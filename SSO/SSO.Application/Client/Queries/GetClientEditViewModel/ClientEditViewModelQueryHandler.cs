namespace SSO.Application.Client.Queries.GetClientEditViewModel
{
    using ApiResource.Queries.GetApiResourceList;
    using Commands.UpdateClient;
    using GetClientDetail;
    using IdentityResource.Queries.GetIdentityResourceList;
    using MediatR;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class ClientEditViewModelQueryHandler : IRequestHandler<ClientEditViewModelQuery, ClientEditViewModel>
    {
        private readonly IMediator _mediator;

        public ClientEditViewModelQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ClientEditViewModel> Handle(ClientEditViewModelQuery request, CancellationToken cancellationToken)
        {
            var viewModel = new ClientEditViewModel
            {
                ApiResources = (await _mediator.Send(new GetApiResourceListQuery())).Select((x) => x.Name),
                IdentityResources = (await _mediator.Send(new GetIdentityResourceListQuery())).Select((x) => x.Name)
            };

            if (!string.IsNullOrEmpty(request.Id))
            {
                var client = await _mediator.Send(new GetClientDetailQuery { Id = request.Id });

                viewModel.Command = (UpdateClientCommand)client;
            }
            else
            {
                viewModel.Command = request.Command;
            }

            return viewModel;
        }
    }
}
