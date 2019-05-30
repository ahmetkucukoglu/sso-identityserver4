namespace SSO.Application.IdentityResource.Commands.DeleteIdentityResource
{
    using Domain.EntityFramework;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteIdentityResourceCommandHandler : IRequestHandler<DeleteIdentityResourceCommand, Unit>
    {
        private readonly AuthDbContext _authContext;

        public DeleteIdentityResourceCommandHandler(AuthDbContext authContext)
        {
            _authContext = authContext;
        }

        public async Task<Unit> Handle(DeleteIdentityResourceCommand request, CancellationToken cancellationToken)
        {
            var identityResource = await _authContext.IdentityResources.FindAsync(request.Name);

            _authContext.IdentityResources.Remove(identityResource);

            await _authContext.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
