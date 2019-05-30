namespace SSO.Application.IdentityResource.Queries.GetIdentityResourceList
{
    using Domain.EntityFramework;
    using MediatR;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetIdentityResourceListQueryHandler : IRequestHandler<GetIdentityResourceListQuery, IEnumerable<IdentityResourceListItem>>
    {
        private readonly AuthDbContext _authContext;

        public GetIdentityResourceListQueryHandler(AuthDbContext authContext)
        {
            _authContext = authContext;
        }

        public Task<IEnumerable<IdentityResourceListItem>> Handle(GetIdentityResourceListQuery request, CancellationToken cancellationToken)
        {
            var identityResources = _authContext.IdentityResources.ToList();

            var result = identityResources.Select((x) => new IdentityResourceListItem
            {
                Description = x.Description,
                DisplayName = x.DisplayName,
                Enabled = x.Enabled,
                Name = x.Name,
                Required = x.Required
            });

            return Task.FromResult(result);
        }
    }
}
