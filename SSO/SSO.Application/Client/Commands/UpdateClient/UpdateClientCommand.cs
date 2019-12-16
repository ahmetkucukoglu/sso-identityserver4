namespace SSO.Application.Client.Commands.UpdateClient
{
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;

    public class UpdateClientCommand : IRequest
    {
        public UpdateClientCommand()
        {
            SelectedIdentityResources = new List<string>();
            SelectedApiResources = new List<string>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public string NewSecret { get; set; } = string.Empty;
        public string PostLogoutRedirectUri { get; set; }
        public string RedirectUri { get; set; }
        public string AllowedCorsOrigin { get; set; }
        public bool RequireConsent { get; set; }
        public string AllowedGrantTypes { get; set; }
        public int Type { get; set; }
        public string LogoUri { get; set; }
        public IFormFile LogoFile { get; set; }
        public IEnumerable<string> SelectedApiResources { get; set; }
        public IEnumerable<string> SelectedIdentityResources { get; set; }
    }
}
