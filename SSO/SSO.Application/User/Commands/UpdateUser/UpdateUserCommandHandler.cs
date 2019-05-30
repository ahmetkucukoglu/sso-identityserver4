namespace SSO.Application.User.Commands.UpdateUser
{
    using SSO.Application.Exceptions;
    using SSO.Domain.Entities;
    using IdentityModel;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateUserCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            var claims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            user.UserName = request.UserName;
            user.Email = request.Email;
            user.EmailConfirmed = request.EmailVerified;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new UserFriendlyException(result.Errors.Select((x) => $"{x.Code}:{x.Description}").ToList());

            result = await _userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
                throw new UserFriendlyException(result.Errors.Select((x) => $"{x.Code}:{x.Description}").ToList());

            var newClaims = new List<System.Security.Claims.Claim> {
                    new System.Security.Claims.Claim(JwtClaimTypes.Name, $"{request.GivenName} {request.FamilyName}"),
                    new System.Security.Claims.Claim(JwtClaimTypes.GivenName, request.GivenName),
                    new System.Security.Claims.Claim(JwtClaimTypes.FamilyName, request.FamilyName)
                };

            result = await _userManager.AddClaimsAsync(user, newClaims);

            if (!result.Succeeded)
                throw new UserFriendlyException(result.Errors.Select((x) => $"{x.Code}:{x.Description}").ToList());

            var addedRoles = request.SelectedRoles.Except(roles);
            var removedRoles = roles.Except(request.SelectedRoles);

            foreach (var item in addedRoles)
            {
                await _userManager.AddToRoleAsync(user, item);
            }

            foreach (var item in removedRoles)
            {
                await _userManager.RemoveFromRoleAsync(user, item);
            }

            return Unit.Value;
        }
    }
}
