namespace SSO.Application.IdentityResource.Queries.GetIdentityResourceDetail
{
    using Domain.EntityFramework;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetIdentityResourceDetailQueryHandler : IRequestHandler<GetIdentityResourceDetailQuery, IdentityResourceDetail>
    {
        private readonly AuthDbContext _authContext;

        public GetIdentityResourceDetailQueryHandler(AuthDbContext authContext)
        {
            _authContext = authContext;
        }

        public async Task<IdentityResourceDetail> Handle(GetIdentityResourceDetailQuery request, CancellationToken cancellationToken)
        {
            var identityResource = await _authContext
               .IdentityResources
               .Include((x) => x.Claims)
               .ThenInclude((x) => x.Claim)
               .FirstOrDefaultAsync((x) => x.Name == request.Name);

            return new IdentityResourceDetail
            {
                Name = identityResource.Name,
                Description = identityResource.Description,
                DisplayName = identityResource.DisplayName,
                Enabled = identityResource.Enabled,
                Required = identityResource.Required,
                SelectedClaims = identityResource.Claims.Select((x) => x.Claim.Type)
            };
        }
    }
}
