namespace SSO.Application.User.Commands.ForgotPassword
{
    using Application.Exceptions;
    using SSO.Infrastructure.Email;
    using Domain.Entities;
    using IdentityModel;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
    {
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public ForgotPasswordCommandHandler(
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IEmailService emailService)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }

        public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                throw new UserFriendlyException("Invalid email");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var uri = new UriBuilder(_httpContextAccessor.HttpContext.Request.Scheme, _httpContextAccessor.HttpContext.Request.Host.Host, _httpContextAccessor.HttpContext.Request.Host.Port.GetValueOrDefault(80), "/Account/ResetPassword");

            uri.Query = $"token={WebUtility.UrlEncode(token)}";

            var claims = await _userManager.GetClaimsAsync(user);
            var name = claims.FirstOrDefault((x) => x.Type == JwtClaimTypes.GivenName)?.Value;

            var result = await _emailService.SendResetPasswordLink(user.Email, name, uri.ToString());

            return result;
        }
    }
}
