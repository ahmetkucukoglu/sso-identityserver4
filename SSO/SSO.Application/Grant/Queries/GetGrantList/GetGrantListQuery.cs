namespace SSO.Application.Grant.Queries.GetGrantList
{
    using MediatR;
    using System.Collections.Generic;

    public class GetGrantListQuery : IRequest<IEnumerable<GrantListItem>> { }
}
