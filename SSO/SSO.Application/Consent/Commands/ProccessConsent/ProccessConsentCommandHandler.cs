namespace SSO.Application.Consent.Commands.ProccessConsent
{
    using Exceptions;
    using IdentityServer4.Models;
    using IdentityServer4.Services;
    using MediatR;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class ProccessConsentCommandHandler : IRequestHandler<ProccessConsentCommand, Unit>
    {
        private readonly IIdentityServerInteractionService _interactionService;

        public ProccessConsentCommandHandler(IIdentityServerInteractionService interactionService)
        {
            _interactionService = interactionService;
        }

        public async Task<Unit> Handle(ProccessConsentCommand request, CancellationToken cancellationToken)
        {
            ConsentResponse consentResponse = null;

            if (request.Action == "no")
            {
                consentResponse = ConsentResponse.Denied;
            }
            else if (request.Action == "yes")
            {
                var scopes = request.ScopesConsented;

                consentResponse = new ConsentResponse
                {
                    RememberConsent = request.RememberConsent,
                    ScopesConsented = scopes.ToArray()
                };
            }

            var authorizationRequest = await _interactionService.GetAuthorizationContextAsync(request.ReturnUrl);

            if (authorizationRequest == null)
                throw new UserFriendlyException("authorizationRequest");

            await _interactionService.GrantConsentAsync(authorizationRequest, consentResponse);

            return Unit.Value;
        }
    }
}
