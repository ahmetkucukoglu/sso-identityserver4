namespace SSO.Application.User.Commands.UpdateUser
{
    using Application.Infrastructure.IdentityServer;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IIdentityUserService _identityUserService;

        public UpdateUserCommandHandler(IIdentityUserService identityUserService)
        {
            _identityUserService = identityUserService;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            await _identityUserService.UpdateUser(request);

            return Unit.Value;
        }
    }
}
