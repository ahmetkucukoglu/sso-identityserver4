namespace SSO.Application.ApiResource.Queries.GetApiResourceDetail
{
    using Domain.EntityFramework;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetApiResourceDetailQueryHandler : IRequestHandler<GetApiResourceDetailQuery, ApiResourceDetail>
    {
        private readonly AuthDbContext _authContext;

        public GetApiResourceDetailQueryHandler(AuthDbContext authContext)
        {
            _authContext = authContext;
        }

        public async Task<ApiResourceDetail> Handle(GetApiResourceDetailQuery request, CancellationToken cancellationToken)
        {
            var apiResource = await _authContext
                .ApiResources
                .Include((x) => x.Claims)
                .ThenInclude((x) => x.Claim)
                .FirstOrDefaultAsync((x) => x.Name == request.Name);

            return new ApiResourceDetail
            {
                Name = apiResource.Name,
                Description = apiResource.Description,
                DisplayName = apiResource.DisplayName,
                Enabled = apiResource.Enabled,
                SelectedClaims = apiResource.Claims.Select((x) => x.Claim.Type)
            };
        }
    }
}
