namespace SSO.Application.Client.Queries.GetClientList
{
    using Domain.EntityFramework;
    using IdentityServer4.Models;
    using MediatR;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetClientListQueryHandler : IRequestHandler<GetClientListQuery, IEnumerable<ClientListItem>>
    {
        private readonly AuthDbContext _authContext;

        public GetClientListQueryHandler(AuthDbContext authContext)
        {
            _authContext = authContext;
        }

        public Task<IEnumerable<ClientListItem>> Handle(GetClientListQuery request, CancellationToken cancellationToken)
        {
            var clients = _authContext.Clients.ToList();

            var result = clients.Select((x) => new ClientListItem
            {
                Enabled = x.Enabled,
                Id = x.Id,
                LogoUri = x.LogoUri,
                Name = x.Name,
                AppType = x.AllowedGrantTypes switch
                {
                    GrantType.Hybrid => "Server Side App",
                    GrantType.Implicit => "Single Page App",
                    GrantType.ClientCredentials => "API",
                    _ => string.Empty
                },
                RequireConsent = x.RequireConsent
            });

            return Task.FromResult(result);
        }
    }
}
