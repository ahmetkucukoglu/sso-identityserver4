namespace SSO.Application.IdentityResource.Queries.GetIdentityResourceEditViewModel
{
    using Claim.Queries.GetClaimList;
    using Commands.UpdateIdentityResource;
    using GetIdentityResourceDetail;
    using MediatR;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class IdentityResourceEditViewModelQueryHandler : IRequestHandler<IdentityResourceEditViewModelQuery, IdentityResourceEditViewModel>
    {
        private readonly IMediator _mediator;

        public IdentityResourceEditViewModelQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IdentityResourceEditViewModel> Handle(IdentityResourceEditViewModelQuery request, CancellationToken cancellationToken)
        {
            var viewModel = new IdentityResourceEditViewModel
            {
                Claims = (await _mediator.Send(new GetClaimListQuery())).Select((x) => x.Type)
            };

            if (!string.IsNullOrEmpty(request.Name))
            {
                var identityResource = await _mediator.Send(new GetIdentityResourceDetailQuery { Name = request.Name });

                viewModel.Command = (UpdateIdentityResourceCommand)identityResource;
            }
            else
            {
                viewModel.Command = request.Command;
            }

            return viewModel;
        }
    }
}
