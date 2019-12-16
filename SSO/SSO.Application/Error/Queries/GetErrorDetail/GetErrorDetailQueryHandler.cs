namespace SSO.Application.Error.Queries.GetErrorDetail
{
    using IdentityServer4.Services;
    using MediatR;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetErrorDetailQueryHandler : IRequestHandler<GetErrorDetailQuery, ErrorDetail>
    {
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public GetErrorDetailQueryHandler(IIdentityServerInteractionService interactionService, IWebHostEnvironment hostingEnvironment)
        {
            _interactionService = interactionService;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<ErrorDetail> Handle(GetErrorDetailQuery request, CancellationToken cancellationToken)
        {
            ErrorDetail errorDetail = null;

            var message = await _interactionService.GetErrorContextAsync(request.Id);

            if (message != null)
            {
                errorDetail = new ErrorDetail
                {
                    Message = message?.Error,
                    Description = _hostingEnvironment.IsDevelopment() ? message?.ErrorDescription : null,
                    RequestId = message?.RequestId
                };
            }

            return errorDetail;
        }
    }
}
