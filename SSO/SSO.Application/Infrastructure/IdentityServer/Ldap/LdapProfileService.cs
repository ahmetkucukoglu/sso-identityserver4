namespace SSO.Application.Infrastructure.IdentityServer.Ldap
{
    using Application.User.Services.Ldap;
    using IdentityServer4.Extensions;
    using IdentityServer4.Models;
    using IdentityServer4.Services;
    using System.Linq;
    using System.Threading.Tasks;

    public class LdapProfileService : IProfileService
    {
        private readonly LdapUserStore _ldapUserStore;

        public LdapProfileService(LdapUserStore ldapUserStore)
        {
            _ldapUserStore = ldapUserStore;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            if (context.RequestedClaimTypes.Any())
            {
                var claims = _ldapUserStore.GetClaims(context.Subject.GetSubjectId());

                context.AddRequestedClaims(claims);
            }

            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var user = _ldapUserStore.GetUser(context.Subject.GetSubjectId());

            context.IsActive = user != null;

            return Task.CompletedTask;
        }
    }
}
