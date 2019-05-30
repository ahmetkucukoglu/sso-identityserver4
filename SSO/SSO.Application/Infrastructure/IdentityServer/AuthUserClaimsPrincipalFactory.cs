namespace SSO.Application.Infrastructure.IdentityServer
{
    using SSO.Domain.Entities;
    using IdentityModel;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    internal class AuthUserClaimsPrincipalFactory : IUserClaimsPrincipalFactory<ApplicationUser>
    {
        private readonly AuthDecorator<IUserClaimsPrincipalFactory<ApplicationUser>> _inner;
        private UserManager<ApplicationUser> _userManager;

        public AuthUserClaimsPrincipalFactory(AuthDecorator<IUserClaimsPrincipalFactory<ApplicationUser>> inner, UserManager<ApplicationUser> userManager)
        {
            _inner = inner;
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await _inner.Instance.CreateAsync(user);
            var identity = principal.Identities.First();

            if (!identity.HasClaim(x => x.Type == JwtClaimTypes.Subject))
            {
                var sub = await _userManager.GetUserIdAsync(user);
                identity.AddClaim(new System.Security.Claims.Claim(JwtClaimTypes.Subject, sub));
            }

            var username = await _userManager.GetUserNameAsync(user);
            var usernameClaim = identity.FindFirst(claim => claim.Type == _userManager.Options.ClaimsIdentity.UserNameClaimType && claim.Value == username);
            if (usernameClaim != null)
            {
                identity.RemoveClaim(usernameClaim);
                identity.AddClaim(new System.Security.Claims.Claim(JwtClaimTypes.PreferredUserName, username));
            }

            if (!identity.HasClaim(x => x.Type == JwtClaimTypes.Name))
            {
                identity.AddClaim(new System.Security.Claims.Claim(JwtClaimTypes.Name, username));
            }

            if (_userManager.SupportsUserEmail)
            {
                var email = await _userManager.GetEmailAsync(user);
                if (!String.IsNullOrWhiteSpace(email))
                {
                    identity.AddClaims(new[]
                    {
                        new System.Security.Claims.Claim(JwtClaimTypes.Email, email),
                        new System.Security.Claims.Claim(JwtClaimTypes.EmailVerified,
                            await _userManager.IsEmailConfirmedAsync(user) ? "true" : "false", ClaimValueTypes.Boolean)
                    });
                }
            }

            if (_userManager.SupportsUserPhoneNumber)
            {
                var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
                if (!String.IsNullOrWhiteSpace(phoneNumber))
                {
                    identity.AddClaims(new[]
                    {
                        new System.Security.Claims.Claim(JwtClaimTypes.PhoneNumber, phoneNumber),
                        new System.Security.Claims.Claim(JwtClaimTypes.PhoneNumberVerified,
                            await _userManager.IsPhoneNumberConfirmedAsync(user) ? "true" : "false", ClaimValueTypes.Boolean)
                    });
                }
            }

            return principal;
        }
    }
}
