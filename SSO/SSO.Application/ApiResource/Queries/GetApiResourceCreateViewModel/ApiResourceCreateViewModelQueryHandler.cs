namespace SSO.Application.ApiResource.Queries.GetApiResourceCreateViewModel
{
    using Claim.Queries.GetClaimList;
    using Commands.CreateApiResource;
    using MediatR;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class ApiResourceCreateViewModelQueryHandler : IRequestHandler<ApiResourceCreateViewModelQuery, ApiResourceCreateViewModel>
    {
        private readonly IMediator _mediator;

        public ApiResourceCreateViewModelQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ApiResourceCreateViewModel> Handle(ApiResourceCreateViewModelQuery request, CancellationToken cancellationToken)
        {
            var viewModel = new ApiResourceCreateViewModel
            {
                Command = request.Command ?? new CreateApiResourceCommand(),
                Claims = (await _mediator.Send(new GetClaimListQuery())).Select((x) => x.Type)
            };

            return viewModel;
        }
    }
}
