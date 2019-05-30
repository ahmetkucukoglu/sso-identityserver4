namespace SSO.Application.Account.Commands.Logout
{
    using SSO.Domain.Entities;
    using IdentityServer4.Events;
    using IdentityServer4.Extensions;
    using IdentityServer4.Services;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using System.Threading;
    using System.Threading.Tasks;

    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, LogoutCommandOutput>
    {
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly IEventService _eventService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogoutCommandHandler(
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interactionService,
            IEventService eventService,
            IHttpContextAccessor httpContextAccessor)
        {
            _interactionService = interactionService;
            _eventService = eventService;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LogoutCommandOutput> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var logout = await _interactionService.GetLogoutContextAsync(request.Id);

            if (_httpContextAccessor.HttpContext.User?.Identity.IsAuthenticated == true)
            {
                //var idp = _httpContextAccessor.HttpContext.User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

                //if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
                //{
                //    var providerSupportsSignout = await _httpContextAccessor.HttpContext.GetSchemeSupportsSignOutAsync(idp);

                //    if (providerSupportsSignout)
                //    {
                //        if (request.Id == null)
                //        {
                //            request.Id = await _interactionService.CreateLogoutContextAsync();
                //        }
                //    }
                //}

                await _signInManager.SignOutAsync();

                await _eventService.RaiseAsync(new UserLogoutSuccessEvent(_httpContextAccessor.HttpContext.User.GetSubjectId(), _httpContextAccessor.HttpContext.User.GetDisplayName()));
            }

            return new LogoutCommandOutput
            {
                AutomaticRedirectAfterSignOut = true,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = request.Id
            };
        }
    }
}
