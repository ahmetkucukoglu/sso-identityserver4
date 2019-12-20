namespace SSO.Application.User.Queries.GetUserDetail
{
    using Application.Infrastructure.IdentityServer;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetUserDetailQueryHandler : IRequestHandler<GetUserDetailQuery, UserDetail>
    {
        private readonly IIdentityUserService _identityUserService;

        public GetUserDetailQueryHandler(IIdentityUserService identityUserService)
        {
            _identityUserService = identityUserService;
        }

        public async Task<UserDetail> Handle(GetUserDetailQuery request, CancellationToken cancellationToken)
        {
            var result = await _identityUserService.GetUserDetail(request);

            return result;
        }
    }
}
