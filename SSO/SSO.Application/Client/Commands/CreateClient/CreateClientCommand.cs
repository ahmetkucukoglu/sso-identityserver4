namespace SSO.Application.Client.Commands.CreateClient
{
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;

    public class CreateClientCommand : IRequest
    {
        private string _idSuffix;

        public CreateClientCommand()
        {
            SelectedIdentityResources = new List<string>();
            SelectedApiResources = new List<string>();
        }

        public string Id { get; set; }
        public string IdSuffix
        {
            get
            {
                if (string.IsNullOrEmpty(_idSuffix))
                    InitClientIdSuffix();

                return _idSuffix;
            }
            set
            {
                _idSuffix = value;
            }
        }
        public string Name { get; set; }
        public string Secret { get; set; }
        public bool Enabled { get; set; }
        public string PostLogoutRedirectUri { get; set; }
        public string RedirectUri { get; set; }
        public string AllowedCorsOrigin { get; set; }
        public bool RequireConsent { get; set; }
        public string AllowedGrantTypes { get; set; }
        public int Type { get; set; } = 1;
        public IEnumerable<string> SelectedApiResources { get; set; }
        public IEnumerable<string> SelectedIdentityResources { get; set; }
        public IFormFile LogoFile { get; set; }

        private void InitClientIdSuffix()
        {
            string clientIdSuffix = "_";

            var characters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'R', 'S', 'T', 'U', 'V', 'Y', 'Z' };

            var random = new Random();

            for (int i = 0; i < 3; i++)
            {
                var chracter = random.Next(0, characters.Length);
                var digit = random.Next(1000, 9999);
                clientIdSuffix += characters[chracter].ToString() + digit;
            };

            _idSuffix = clientIdSuffix;
        }
    }
}
