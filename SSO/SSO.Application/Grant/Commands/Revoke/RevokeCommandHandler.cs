namespace SSO.Application.Grant.Commands.Revoke
{
    using IdentityServer4.Services;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class RevokeCommandHandler : IRequestHandler<RevokeCommand, Unit>
    {
        private readonly IIdentityServerInteractionService _interactionService;

        public RevokeCommandHandler(IIdentityServerInteractionService interactionService)
        {
            _interactionService = interactionService;
        }

        public async Task<Unit> Handle(RevokeCommand request, CancellationToken cancellationToken)
        {
            await _interactionService.RevokeUserConsentAsync(request.ClientId);

            return Unit.Value;
        }
    }
}
