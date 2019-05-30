namespace SSO.Application.Role.Queries.GetRoleList
{
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetRoleListQueryHandler : IRequestHandler<GetRoleListQuery, IEnumerable<RoleListItem>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public GetRoleListQueryHandler(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public Task<IEnumerable<RoleListItem>> Handle(GetRoleListQuery request, CancellationToken cancellationToken)
        {
            var roles = _roleManager.Roles.ToList();

            var result = roles.Select((x) => new RoleListItem
            {
                Name = x.Name
            });

            return Task.FromResult(result);
        }
    }
}
