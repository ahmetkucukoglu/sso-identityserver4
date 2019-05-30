namespace SSO.Application.Infrastructure.IdentityServer.Extensions
{
    using Domain.Entities;
    using System.Linq;

    public static class IdentityResourceExtensions
    {
        public static IdentityServer4.Models.IdentityResource GetIdentityResource(this IdentityResource identityResource)
        {
            var claims = identityResource.Claims.Select((x) => x.Claim.Type);

            var resource = new IdentityServer4.Models.IdentityResource(identityResource.Name, identityResource.DisplayName, claims)
            {
                Description = identityResource.Description,
                Enabled = identityResource.Enabled,
                Required = identityResource.Required
            };

            return resource;
        }
    }
}
