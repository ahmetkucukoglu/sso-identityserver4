namespace SSO.Application.Claim.Queries.GetClaimList
{
    using Domain.EntityFramework;
    using MediatR;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetClaimListQueryHandler : IRequestHandler<GetClaimListQuery, IEnumerable<ClaimListItem>>
    {
        private readonly AuthDbContext _authContext;

        public GetClaimListQueryHandler(AuthDbContext authContext)
        {
            _authContext = authContext;
        }

        public Task<IEnumerable<ClaimListItem>> Handle(GetClaimListQuery request, CancellationToken cancellationToken)
        {
            var claims = _authContext.Claims.ToList();

            var result = claims.Select((x) => new ClaimListItem
            {
                Type = x.Type
            });

            return Task.FromResult(result);
        }
    }
}
