namespace SSO.Application.Infrastructure.IdentityServer
{
    using Domain.EntityFramework;
    using Extensions;
    using IdentityServer4.Models;
    using IdentityServer4.Stores;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PersistedGrantStore : IPersistedGrantStore
    {
        private readonly AuthDbContext _authContext;

        public PersistedGrantStore(AuthDbContext authContext)
        {
            _authContext = authContext;
        }

        public Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            var grants = _authContext.Grants
                //.Where((x) => x.SubjectId == subjectId)
                .Select((x) => x.GetPersistedGrant())
                .AsEnumerable();

            return Task.FromResult(grants);
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            var grant = await _authContext.Grants.FindAsync(key);

            return grant?.GetPersistedGrant();
        }

        public async Task RemoveAllAsync(string subjectId, string clientId)
        {
            var grants = _authContext.Grants
                .Where((x) => x.ClientId == clientId);
              //.Where((x) => x.SubjectId == subjectId && x.ClientId == clientId);

            _authContext.Grants.RemoveRange(grants);

            await _authContext.SaveChangesAsync();
        }

        public async Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            var grants = _authContext.Grants
                .Where((x) => x.ClientId == clientId && x.Type == type);
              //.Where((x) => x.SubjectId == subjectId && x.ClientId == clientId && x.Type == type);

            _authContext.Grants.RemoveRange(grants);

            await _authContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(string key)
        {
            var grant = await _authContext.Grants.FindAsync(key);

            _authContext.Grants.Remove(grant);

            await _authContext.SaveChangesAsync();
        }

        public async Task StoreAsync(PersistedGrant persistedGrant)
        {
            var grant = await _authContext.Grants.FindAsync(persistedGrant.Key);

            if (grant == null)
            {
                await _authContext.Grants.AddAsync(persistedGrant.CreateGrant());
            }
            else
            {
                persistedGrant.UpdateGrant(grant);
            }

            await _authContext.SaveChangesAsync();
        }
    }
}
