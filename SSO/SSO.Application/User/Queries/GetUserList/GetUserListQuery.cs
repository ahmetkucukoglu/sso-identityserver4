namespace SSO.Application.User.Queries.GetUserList
{
    using MediatR;
    using System.Collections.Generic;

    public class GetUserListQuery : IRequest<IEnumerable<UserListItem>> { }
}
