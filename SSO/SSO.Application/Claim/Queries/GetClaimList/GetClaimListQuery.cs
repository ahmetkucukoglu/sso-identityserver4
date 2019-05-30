namespace SSO.Application.Claim.Queries.GetClaimList
{
    using MediatR;
    using System.Collections.Generic;

    public class GetClaimListQuery : IRequest<IEnumerable<ClaimListItem>> { }
}
