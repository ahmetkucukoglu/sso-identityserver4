namespace SSO.Application.User.Commands.DeleteUser
{
    using MediatR;
    using Application.Infrastructure.IdentityServer;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IIdentityUserService _identityUserService;

        public DeleteUserCommandHandler(IIdentityUserService identityUserService)
        {
            _identityUserService = identityUserService;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await _identityUserService.DeleteUser(request);

            return Unit.Value;
        }
    }
}
