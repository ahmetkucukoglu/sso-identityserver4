namespace SSO.Application.User.Commands.ResetPassword
{
    using Application.Infrastructure.IdentityServer;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Unit>
    {
        private readonly IIdentityUserService _identityUserService;

        public ResetPasswordCommandHandler(IIdentityUserService identityUserService)
        {
            _identityUserService = identityUserService;
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            await _identityUserService.ResetPassword(request);

            return Unit.Value;
        }
    }
}
