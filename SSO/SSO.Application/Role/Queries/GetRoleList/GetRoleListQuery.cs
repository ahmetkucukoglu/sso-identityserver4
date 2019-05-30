namespace SSO.Application.Role.Queries.GetRoleList
{
    using MediatR;
    using System.Collections.Generic;

    public class GetRoleListQuery : IRequest<IEnumerable<RoleListItem>> { }
}
