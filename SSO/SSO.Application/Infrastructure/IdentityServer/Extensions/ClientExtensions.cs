namespace SSO.Application.Infrastructure.IdentityServer.Extensions
{
    using Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ClientExtensions
    {
        public static IdentityServer4.Models.Client GetClient(this Client client)
        {
            var redirectUris = client.RedirectUri?.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList(); //TODO Refactoring
            var postLogoutRedirectUris = client.PostLogoutRedirectUri?.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList(); //TODO Refactoring
            var allowedCorsOrigins = client.AllowedCorsOrigin?.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList(); //TODO Refactoring
            
            var identityClient = new IdentityServer4.Models.Client
            {
                ClientId = client.Id,
                AllowedGrantTypes = new List<string> { client.AllowedGrantTypes },
                ClientName = client.Name,
                ClientSecrets = new List<IdentityServer4.Models.Secret>
                {
                    new IdentityServer4.Models.Secret(client.ClientSecret)
                },
                Enabled = client.Enabled,
                RedirectUris = redirectUris,
                PostLogoutRedirectUris = postLogoutRedirectUris,
                RequireConsent = client.RequireConsent,
                LogoUri = client.LogoUri,
                RefreshTokenUsage = IdentityServer4.Models.TokenUsage.ReUse,
                RefreshTokenExpiration = IdentityServer4.Models.TokenExpiration.Sliding,
                UpdateAccessTokenClaimsOnRefresh = true,
                AllowOfflineAccess = true,
                RequireClientSecret = client.AllowedGrantTypes != IdentityServer4.Models.GrantType.Implicit,
                AllowAccessTokensViaBrowser = client.AllowedGrantTypes == IdentityServer4.Models.GrantType.Implicit,
                AllowedCorsOrigins = allowedCorsOrigins
            };

            var identityResources = client.IdentityResources.Select((x) => x.IdentityResource.Name).ToList();
            var apiResources = client.ApiResources.Select((x) => x.ApiResource.Name).ToList();

            identityResources.AddRange(apiResources);

            identityClient.AllowedScopes = identityResources;

            return identityClient;
        }
    }
}
