namespace SSO.Application.Infrastructure.IdentityServer
{
    using Domain.EntityFramework;
    using Extensions;
    using IdentityServer4.Models;
    using IdentityServer4.Stores;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ResourceStore : IResourceStore
    {
        private readonly AuthDbContext _authContext;

        public ResourceStore(AuthDbContext authContext)
        {
            _authContext = authContext;
        }

        public async Task<ApiResource> FindApiResourceAsync(string name)
        {
            var apiResource = await _authContext.ApiResources
                .Include((x) => x.Claims)
                .ThenInclude((x) => x.Claim)
                .FirstOrDefaultAsync((x) => x.Name == name);

            return apiResource?.GetApiResource();
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var apiResources = _authContext.ApiResources
                .Include((x) => x.Claims)
                .ThenInclude((x) => x.Claim)
                .Where((x) => scopeNames.Contains(x.Name))
                .Select((x) => x.GetApiResource())
                .AsEnumerable();

            return Task.FromResult(apiResources);
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var identityResources = _authContext.IdentityResources
                .Include((x) => x.Claims)
                .ThenInclude((x) => x.Claim)
                .Where((x) => scopeNames.Contains(x.Name))
                .Select((x) => x.GetIdentityResource())
                .AsEnumerable();

            return Task.FromResult(identityResources);
        }

        public Task<Resources> GetAllResourcesAsync()
        {
            var apiResources = _authContext.ApiResources
                .Include((x) => x.Claims)
                .ThenInclude((x) => x.Claim)
                .Select((x) => x.GetApiResource());

            var identityResources = _authContext.IdentityResources
                .Include((x) => x.Claims)
                .ThenInclude((x) => x.Claim)
                .Select((x) => x.GetIdentityResource());

            var resources = new Resources(identityResources, apiResources);

            return Task.FromResult(resources);
        }
    }
}
