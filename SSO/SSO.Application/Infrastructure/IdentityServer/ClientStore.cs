namespace SSO.Application.Infrastructure.IdentityServer
{
    using Domain.EntityFramework;
    using Extensions;
    using IdentityServer4.Models;
    using IdentityServer4.Stores;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class ClientStore : IClientStore
    {
        private readonly AuthDbContext _authContext;

        public ClientStore(AuthDbContext authContext)
        {
            _authContext = authContext;
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = await _authContext.Clients
                .Include((x) => x.IdentityResources)
                .ThenInclude((x) => x.IdentityResource)
                .Include((x) => x.ApiResources)
                .ThenInclude((x) => x.ApiResource)
                .FirstOrDefaultAsync((x) => x.Id == clientId);

            return client?.GetClient();
        }
    }
}
