namespace SSO.Application.Account.Queries.GetLoginDetail
{
    using IdentityServer4.Services;
    using IdentityServer4.Stores;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetLoginDetailQueryHandler : IRequestHandler<GetLoginDetailQuery, LoginDetail>
    {
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly IClientStore _clientStore;

        public GetLoginDetailQueryHandler(
            IIdentityServerInteractionService interactionService,
            IClientStore clientStore)
        {
            _interactionService = interactionService;
            _clientStore = clientStore;
        }

        public async Task<LoginDetail> Handle(GetLoginDetailQuery request, CancellationToken cancellationToken)
        {
            var context = await _interactionService.GetAuthorizationContextAsync(request.ReturnUrl);

            var allowLocal = true;

            if (context?.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);

                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;
                }
            }

            return new LoginDetail
            {
                AllowRememberLogin = true,
                EnableLocalLogin = allowLocal,
                ReturnUrl = request.ReturnUrl,
                UserName = context?.LoginHint
            };
        }
    }
}
