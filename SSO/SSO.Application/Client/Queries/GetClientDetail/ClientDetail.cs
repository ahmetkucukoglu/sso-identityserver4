namespace SSO.Application.Client.Queries.GetClientDetail
{
    using Commands.UpdateClient;
    using System.Collections.Generic;

    public class ClientDetail
    {
        public ClientDetail()
        {
            SelectedIdentityResources = new List<string>();
            SelectedApiResources = new List<string>();
        }

        public string Id { get; set; }
        public string IdSuffix { get; set; } = string.Empty;
        public string Name { get; set; }
        public string Secret { get; set; }
        public string NewSecret { get; set; } = string.Empty;
        public bool Enabled { get; set; }
        public string LogoUri { get; set; }
        public string PostLogoutRedirectUri { get; set; }
        public string RedirectUri { get; set; }
        public string AllowedCorsOrigin { get; set; }
        public bool RequireConsent { get; set; }
        public bool RequireClientSecret { get; set; }
        public string AllowedGrantTypes { get; set; }
        public int Type { get { return !string.IsNullOrEmpty(RedirectUri) ? 1 : 2; } }
        public IEnumerable<string> SelectedApiResources { get; set; }
        public IEnumerable<string> SelectedIdentityResources { get; set; }

        public static explicit operator UpdateClientCommand(ClientDetail clientDetail)
        {
            return new UpdateClientCommand
            {
                AllowedGrantTypes = clientDetail.AllowedGrantTypes,
                Enabled = clientDetail.Enabled,
                Id = clientDetail.Id,
                Name = clientDetail.Name,
                NewSecret = clientDetail.NewSecret,
                LogoUri = clientDetail.LogoUri,
                PostLogoutRedirectUri = clientDetail.PostLogoutRedirectUri,
                RedirectUri = clientDetail.RedirectUri,
                AllowedCorsOrigin = clientDetail.AllowedCorsOrigin,
                RequireConsent = clientDetail.RequireConsent,
                RequireClientSecret = clientDetail.RequireClientSecret,
                Type = clientDetail.Type,
                SelectedApiResources = clientDetail.SelectedApiResources,
                SelectedIdentityResources = clientDetail.SelectedIdentityResources
            };
        }
    }
}
