namespace SSO.Application.Account.Commands.Login
{
    using SSO.Application.Exceptions;
    using Domain.Entities;
    using IdentityServer4.Events;
    using IdentityServer4.Models;
    using IdentityServer4.Services;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using System.Threading;
    using System.Threading.Tasks;

    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly IEventService _eventService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginCommandHandler(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interactionService,
            IEventService eventService)
        {
            _interactionService = interactionService;
            _eventService = eventService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            string returnUrl = string.Empty;

            if (request.Action != "login")
            {
                var context = await _interactionService.GetAuthorizationContextAsync(request.ReturnUrl);

                if (context != null)
                {
                    await _interactionService.GrantConsentAsync(context, ConsentResponse.Denied);

                    returnUrl = request.ReturnUrl;
                }
                else
                {
                    returnUrl = "~/";
                }
            }
            else
            {
                var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, request.RememberLogin, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(request.Username);

                    await _eventService.RaiseAsync(new UserLoginSuccessEvent(request.Username, user.Id, request.Username));

                    if (_interactionService.IsValidReturnUrl(request.ReturnUrl))
                    {
                        returnUrl = request.ReturnUrl;
                    }
                    else
                    {
                        returnUrl = "~/";
                    }
                }
                else
                {
                    await _eventService.RaiseAsync(new UserLoginFailureEvent(request.Username, "invalid credentials"));

                    throw new UserFriendlyException("Invalid username or password");
                }
            }

            return returnUrl;
        }
    }
}
