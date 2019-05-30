namespace SSO.Application.ApiResource.Queries.GetApiResourceList
{
    using MediatR;
    using System.Collections.Generic;

    public class GetApiResourceListQuery : IRequest<IEnumerable<ApiResourceListItem>> { }
}
