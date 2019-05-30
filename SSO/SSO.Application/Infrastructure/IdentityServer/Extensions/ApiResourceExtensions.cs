namespace SSO.Application.Infrastructure.IdentityServer.Extensions
{
    using Domain.Entities;
    using System.Linq;

    public static class ApiResourceExtensions
    {
        public static IdentityServer4.Models.ApiResource GetApiResource(this ApiResource apiResource)
        {
            var claims = apiResource.Claims.Select((x) => x.Claim.Type);

            var resource = new IdentityServer4.Models.ApiResource(apiResource.Name, apiResource.DisplayName, claims)
            {
                Description = apiResource.Description,
                Enabled = apiResource.Enabled
            };

            return resource;
        }
    }
}
