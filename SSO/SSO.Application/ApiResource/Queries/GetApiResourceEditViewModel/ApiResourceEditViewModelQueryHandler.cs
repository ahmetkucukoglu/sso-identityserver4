namespace SSO.Application.ApiResource.Queries.GetApiResourceEditViewModel
{
    using Claim.Queries.GetClaimList;
    using Commands.UpdateApiResource;
    using GetApiResourceDetail;
    using MediatR;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class ApiResourceEditViewModelQueryHandler : IRequestHandler<ApiResourceEditViewModelQuery, ApiResourceEditViewModel>
    {
        private readonly IMediator _mediator;

        public ApiResourceEditViewModelQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ApiResourceEditViewModel> Handle(ApiResourceEditViewModelQuery request, CancellationToken cancellationToken)
        {
            var viewModel = new ApiResourceEditViewModel
            {
                Claims = (await _mediator.Send(new GetClaimListQuery())).Select((x) => x.Type)
            };

            if (!string.IsNullOrEmpty(request.Name))
            {
                var apiResource = await _mediator.Send(new GetApiResourceDetailQuery { Name = request.Name });

                viewModel.Command = (UpdateApiResourceCommand)apiResource;
            }
            else
            {
                viewModel.Command = request.Command;
            }

            return viewModel;
        }
    }
}
