namespace SSO.Application.Infrastructure.IdentityServer
{
    using SSO.Domain.Entities;
    using IdentityServer4.Extensions;
    using IdentityServer4.Models;
    using IdentityServer4.Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    public class AuthProfileService : IProfileService
    {
        protected readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        protected readonly ILogger<AuthProfileService> _logger;
        protected readonly UserManager<ApplicationUser> _userManager;

        public AuthProfileService(
            UserManager<ApplicationUser> userManager,
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public AuthProfileService(
            UserManager<ApplicationUser> userManager,
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
            ILogger<AuthProfileService> logger)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
            _logger = logger;
        }

        public virtual async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject?.GetSubjectId();

            if (sub == null) throw new Exception("No sub claim present");

            var user = await _userManager.FindByIdAsync(sub);

            if (user == null)
            {
                _logger?.LogWarning("No user found matching subject Id: {0}", sub);
            }
            else
            {
                var principal = await _claimsFactory.CreateAsync(user);
                if (principal == null) throw new Exception("ClaimsFactory failed to create a principal");

                context.AddRequestedClaims(principal.Claims);
            }
        }

        public virtual async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject?.GetSubjectId();

            if (sub == null) throw new Exception("No subject Id claim present");

            var user = await _userManager.FindByIdAsync(sub);

            if (user == null)
            {
                _logger?.LogWarning("No user found matching subject Id: {0}", sub);
            }

            context.IsActive = user != null;
        }
    }
}
