namespace SSO.Application.Consent.Queries.GetConsentDetail
{
    using Exceptions;
    using IdentityServer4.Services;
    using IdentityServer4.Stores;
    using MediatR;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetConsentDetailQueryHandler : IRequestHandler<GetConsentDetailQuery, ConsentDetail>
    {
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;

        public GetConsentDetailQueryHandler(
            IIdentityServerInteractionService interactionService,
            IClientStore clientStore,
            IResourceStore resourceStore)
        {
            _interactionService = interactionService;
            _clientStore = clientStore;
            _resourceStore = resourceStore;
        }

        public async Task<ConsentDetail> Handle(GetConsentDetailQuery request, CancellationToken cancellationToken)
        {
            var authorizationRequest = await _interactionService.GetAuthorizationContextAsync(request.ReturnUrl);

            if (authorizationRequest == null)
                throw new UserFriendlyException("authorizationRequest");

            var client = await _clientStore.FindEnabledClientByIdAsync(authorizationRequest.ClientId);

            if (client == null)
                throw new UserFriendlyException("client");

            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(authorizationRequest.ScopesRequested);

            if (resources == null || (!resources.IdentityResources.Any() && !resources.ApiResources.Any()))
                throw new UserFriendlyException("resources");

            var consentDetail = new ConsentDetail
            {
                ClientName = client.ClientName ?? client.ClientId,
                ClientUrl = client.ClientUri,
                ClientLogoUrl = client.LogoUri,
                AllowRememberConsent = client.AllowRememberConsent,
                RememberConsent = client.AllowRememberConsent,
                ReturnUrl = request.ReturnUrl
            };

            consentDetail.IdentityScopes = resources.IdentityResources.Select(x => new ScopeDetail
            {
                Name = x.Name,
                DisplayName = x.DisplayName,
                Description = x.Description,
                Emphasize = x.Emphasize,
                Required = x.Required,
                Checked = x.Required
            }).ToArray();

            consentDetail.ResourceScopes = resources.ApiResources.SelectMany(x => x.Scopes).Select(x => new ScopeDetail
            {
                Name = x.Name,
                DisplayName = x.DisplayName,
                Description = x.Description,
                Emphasize = x.Emphasize,
                Required = x.Required,
                Checked = x.Required
            }).ToArray();

            if (resources.OfflineAccess)
            {
                consentDetail.ResourceScopes = consentDetail.ResourceScopes.Union(new ScopeDetail[] { new ScopeDetail {
                    Name = IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess,
                    DisplayName = "Offline Access",
                    Description = "Access to your applications and resources, even when you are offline",
                    Emphasize = true,
                    Checked = true
                }});
            }

            return consentDetail;
        }
    }
}
