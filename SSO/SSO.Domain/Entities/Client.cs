namespace SSO.Domain.Entities
{
    using System.Collections.Generic;

    public class Client
    {
        public Client()
        {
            IdentityResources = new List<ClientIdentityResource>();
            ApiResources = new List<ClientApiResource>();
        }

        public string Id { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string AllowedGrantTypes { get; set; }
        public bool RequireConsent { get; set; }
        public string ClientSecret { get; set; }
        public string LogoUri { get; set; }
        public string RedirectUri { get; set; }
        public string PostLogoutRedirectUri { get; set; }
        public string AllowedCorsOrigin { get; set; }
        public ICollection<ClientIdentityResource> IdentityResources { get; set; }
        public ICollection<ClientApiResource> ApiResources { get; set; }
    }
}
