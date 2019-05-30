namespace SSO.Application.IdentityResource.Queries.GetIdentityResourceList
{
    using MediatR;
    using System.Collections.Generic;

    public class GetIdentityResourceListQuery : IRequest<IEnumerable<IdentityResourceListItem>> { }
}
