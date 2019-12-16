namespace SSO.Application.Client.Queries.GetClientDetail
{
    using Domain.EntityFramework;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetClientDetailQueryHandler : IRequestHandler<GetClientDetailQuery, ClientDetail>
    {
        private readonly AuthDbContext _authContext;

        public GetClientDetailQueryHandler(AuthDbContext authContext)
        {
            _authContext = authContext;
        }

        public async Task<ClientDetail> Handle(GetClientDetailQuery request, CancellationToken cancellationToken)
        {
            var client = await _authContext
                .Clients
                .Include((x) => x.IdentityResources)
                .ThenInclude((x) => x.IdentityResource)
                .Include((x) => x.ApiResources)
                .ThenInclude((x) => x.ApiResource)
                .FirstOrDefaultAsync((x) => x.Id == request.Id);

            return new ClientDetail
            {
                Id = client.Id,
                Name = client.Name,
                Secret = client.ClientSecret,
                LogoUri = client.LogoUri,
                PostLogoutRedirectUri = client.PostLogoutRedirectUri,
                RedirectUri = client.RedirectUri,
                AllowedCorsOrigin = client.AllowedCorsOrigin,
                RequireConsent = client.RequireConsent,
                AllowedGrantTypes = client.AllowedGrantTypes,
                Enabled = client.Enabled,
                SelectedApiResources = client.ApiResources.Select((x) => x.ApiResource.Name),
                SelectedIdentityResources = client.IdentityResources.Select((x) => x.IdentityResource.Name)
            };
        }
    }
}
