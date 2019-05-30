namespace SSO.Application.IdentityResource.Queries.GetIdentityResourceCreateViewModel
{
    using Claim.Queries.GetClaimList;
    using Commands.CreateIdentityResource;
    using MediatR;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class IdentityResourceCreateViewModelQueryHandler : IRequestHandler<IdentityResourceCreateViewModelQuery, IdentityResourceCreateViewModel>
    {
        private readonly IMediator _mediator;

        public IdentityResourceCreateViewModelQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IdentityResourceCreateViewModel> Handle(IdentityResourceCreateViewModelQuery request, CancellationToken cancellationToken)
        {
            var viewModel = new IdentityResourceCreateViewModel
            {
                Command = request.Command ?? new CreateIdentityResourceCommand(),
                Claims = (await _mediator.Send(new GetClaimListQuery())).Select((x) => x.Type)
            };

            return viewModel;
        }
    }
}
