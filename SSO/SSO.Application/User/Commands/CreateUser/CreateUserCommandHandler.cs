namespace SSO.Application.User.Commands.CreateUser
{
    using Application.Exceptions;
    using Domain.Entities;
    using IdentityModel;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Unit>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateUserCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
                EmailConfirmed = request.EmailVerified
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                throw new UserFriendlyException(result.Errors.Select((x) => $"{x.Code}:{x.Description}").ToList());

            var claims = new List<System.Security.Claims.Claim> {
                new System.Security.Claims.Claim(JwtClaimTypes.Name, $"{request.GivenName} {request.FamilyName}"),
                new System.Security.Claims.Claim(JwtClaimTypes.GivenName, request.GivenName),
                new System.Security.Claims.Claim(JwtClaimTypes.FamilyName, request.FamilyName)
            };

            result = await _userManager.AddClaimsAsync(user, claims);

            if (!result.Succeeded)
                throw new UserFriendlyException(result.Errors.Select((x) => $"{x.Code}:{x.Description}").ToList());

            foreach (var item in request.SelectedRoles)
            {
                await _userManager.AddToRoleAsync(user, item);
            }

            return Unit.Value;
        }
    }
}
