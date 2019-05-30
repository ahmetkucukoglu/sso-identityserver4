namespace SSO.Application.Client.Queries.GetClientList
{
    using MediatR;
    using System.Collections.Generic;

    public class GetClientListQuery : IRequest<IEnumerable<ClientListItem>> { }
}
