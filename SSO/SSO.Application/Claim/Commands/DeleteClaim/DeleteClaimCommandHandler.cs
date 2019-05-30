namespace SSO.Application.Claim.Commands.DeleteClaim
{
    using Domain.EntityFramework;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteClaimCommandHandler : IRequestHandler<DeleteClaimCommand, Unit>
    {
        private readonly AuthDbContext _authContext;

        public DeleteClaimCommandHandler(AuthDbContext authContext)
        {
            _authContext = authContext;
        }

        public async Task<Unit> Handle(DeleteClaimCommand request, CancellationToken cancellationToken)
        {
            var claim = await _authContext.Claims.FindAsync(request.Type);

            _authContext.Claims.Remove(claim);

            await _authContext.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
