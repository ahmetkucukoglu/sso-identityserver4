namespace SSO.Application.Consent.Queries.GetConsentDetail
{
    using System.Collections.Generic;

    public class ConsentDetail
    {
        public string ClientName { get; set; }
        public string ClientUrl { get; set; }
        public string ClientLogoUrl { get; set; }
        public bool AllowRememberConsent { get; set; }
        public bool RememberConsent { get; set; }
        public string ReturnUrl { get; set; }
        public IEnumerable<ScopeDetail> IdentityScopes { get; set; }
        public IEnumerable<ScopeDetail> ResourceScopes { get; set; }
    }
}
