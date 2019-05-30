namespace SSO.Application.Infrastructure.IdentityServer
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class SecurityStampValidatorCallback
    {
        public static Task UpdatePrincipal(SecurityStampRefreshingPrincipalContext context)
        {
            var newClaimTypes = context.NewPrincipal.Claims.Select(x => x.Type).ToArray();
            var currentClaimsToKeep = context.CurrentPrincipal.Claims.Where(x => !newClaimTypes.Contains(x.Type)).ToArray();

            var id = context.NewPrincipal.Identities.First();
            id.AddClaims(currentClaimsToKeep);

            return Task.CompletedTask;
        }
    }
}
