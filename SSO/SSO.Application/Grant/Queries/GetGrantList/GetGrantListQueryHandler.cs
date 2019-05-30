namespace SSO.Application.Grant.Queries.GetGrantList
{
    using IdentityServer4.Services;
    using IdentityServer4.Stores;
    using MediatR;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetGrantListQueryHandler : IRequestHandler<GetGrantListQuery, IEnumerable<GrantListItem>>
    {
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;

        public GetGrantListQueryHandler(
            IIdentityServerInteractionService interactionService,
            IClientStore clientStore,
            IResourceStore resourceStore)
        {
            _interactionService = interactionService;
            _clientStore = clientStore;
            _resourceStore = resourceStore;
        }

        public async Task<IEnumerable<GrantListItem>> Handle(GetGrantListQuery request, CancellationToken cancellationToken)
        {
            var result = new List<GrantListItem>();

            var grants = await _interactionService.GetAllUserConsentsAsync();

            foreach (var grant in grants)
            {
                var client = await _clientStore.FindClientByIdAsync(grant.ClientId);

                if (client != null)
                {
                    var resources = await _resourceStore.FindResourcesByScopeAsync(grant.Scopes);

                    result.Add(new GrantListItem
                    {
                        ClientId = client.ClientId,
                        ClientName = client.ClientName ?? client.ClientId,
                        ClientLogoUrl = client.LogoUri,
                        ClientUrl = client.LogoUri,
                        Created = grant.CreationTime,
                        Expires = grant.Expiration,
                        IdentityGrantNames = resources.IdentityResources.Select(x => x.DisplayName ?? x.Name).ToArray(),
                        ApiGrantNames = resources.ApiResources.Select(x => x.DisplayName ?? x.Name).ToArray()
                    });
                }
            }

            return result;
        }
    }
}
