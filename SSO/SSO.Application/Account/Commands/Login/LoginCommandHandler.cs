namespace SSO.Application.Account.Commands.Login
{
    using Application.Infrastructure.IdentityServer;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IIdentityUserService _identityUserService;

        public LoginCommandHandler(IIdentityUserService identityUserService)
        {
            _identityUserService = identityUserService;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            string returnUrl = await _identityUserService.Login(request);

            return returnUrl;
        }
    }
}
