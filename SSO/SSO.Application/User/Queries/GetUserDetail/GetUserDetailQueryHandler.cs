namespace SSO.Application.User.Queries.GetUserDetail
{
    using SSO.Domain.Entities;
    using IdentityModel;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetUserDetailQueryHandler : IRequestHandler<GetUserDetailQuery, UserDetail>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public GetUserDetailQueryHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<UserDetail> Handle(GetUserDetailQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            var claims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            return new UserDetail
            {
                Id = user.Id,
                UserName = user.UserName,
                GivenName = claims.FirstOrDefault((x) => x.Type == JwtClaimTypes.GivenName)?.Value,
                FamilyName = claims.FirstOrDefault((x) => x.Type == JwtClaimTypes.FamilyName)?.Value,
                Email = user.Email,
                SelectedRoles = roles,
                EmailVerified = user.EmailConfirmed
            };
        }
    }
}
