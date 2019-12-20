namespace SSO.Application.User.Queries.GetUserList
{
    using Application.Infrastructure.IdentityServer;
    using MediatR;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetGrantListQueryHandler : IRequestHandler<GetUserListQuery, IEnumerable<UserListItem>>
    {
        private readonly IIdentityUserService _identityUserService;

        public GetGrantListQueryHandler(IIdentityUserService identityUserService)
        {
            _identityUserService = identityUserService;
        }

        public async Task<IEnumerable<UserListItem>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var result = await _identityUserService.GetUserList(request);

            return result;
        }
    }
}
