namespace SSO.Application.Client.Queries.GetClientList
{
    using Domain.EntityFramework;
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
                RequireConsent = x.RequireConsent
            });

            return Task.FromResult(result);
        }
    }
}
