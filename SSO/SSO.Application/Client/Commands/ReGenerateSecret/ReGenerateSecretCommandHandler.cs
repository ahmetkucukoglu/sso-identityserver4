namespace SSO.Application.Client.Commands.ReGenerateSecret
{
    using Domain.EntityFramework;
    using IdentityModel;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Threading;
    using System.Threading.Tasks;

    public class ReGenerateSecretCommandHandler : IRequestHandler<ReGenerateSecretCommand, Unit>
    {
        private readonly AuthDbContext _authContext;

        public ReGenerateSecretCommandHandler(AuthDbContext authContext)
        {
            _authContext = authContext;
        }

        public async Task<Unit> Handle(ReGenerateSecretCommand request, CancellationToken cancellationToken)
        {
            var client = await _authContext.Clients
                    .FirstOrDefaultAsync((x) => x.Id == request.Id);

            client.ClientSecret = request.NewSecret.ToSha256();

            _authContext.Update(client);

            await _authContext.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
