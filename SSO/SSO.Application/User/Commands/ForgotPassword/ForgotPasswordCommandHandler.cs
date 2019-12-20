namespace SSO.Application.User.Commands.ForgotPassword
{
    using Application.Infrastructure.IdentityServer;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
    {
        private readonly IIdentityUserService _identityUserService;

        public ForgotPasswordCommandHandler(IIdentityUserService identityUserService)
        {
            _identityUserService = identityUserService;
        }

        public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityUserService.ForgotPassword(request);

            return result;
        }
    }
}
