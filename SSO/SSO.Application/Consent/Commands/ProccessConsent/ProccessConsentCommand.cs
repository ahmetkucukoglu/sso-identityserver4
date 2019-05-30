namespace SSO.Application.Consent.Commands.ProccessConsent
{
    using MediatR;
    using System.Collections.Generic;

    public class ProccessConsentCommand : IRequest
    {
        public IEnumerable<string> ScopesConsented { get; set; }
        public bool RememberConsent { get; set; }
        public string ReturnUrl { get; set; }
        public string Action { get; set; }
    }
}
