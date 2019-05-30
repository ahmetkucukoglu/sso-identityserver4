namespace SSO.Application.User.Queries.GetUserList
{
    using SSO.Domain.Entities;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetGrantListQueryHandler : IRequestHandler<GetUserListQuery, IEnumerable<UserListItem>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetGrantListQueryHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserListItem>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var result = new List<UserListItem>();

            var users = _userManager.Users.ToList();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                result.Add(new UserListItem
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles.Aggregate((x, y) => x + "," + y)
                });
            }

            return result;
        }
    }
}
