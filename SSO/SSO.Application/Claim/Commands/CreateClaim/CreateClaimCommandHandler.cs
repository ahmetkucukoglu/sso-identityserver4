namespace SSO.Application.Claim.Commands.CreateClaim
{
    using Domain.EntityFramework;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateClaimCommandHandler : IRequestHandler<CreateClaimCommand, Unit>
    {
        private readonly AuthDbContext _authContext;

        public CreateClaimCommandHandler(AuthDbContext authContext)
        {
            _authContext = authContext;
        }

        public async Task<Unit> Handle(CreateClaimCommand request, CancellationToken cancellationToken)
        {
            var claim = new Domain.Entities.Claim
            {
                Type = request.Type
            };

            await _authContext.Claims.AddAsync(claim);

            await _authContext.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
