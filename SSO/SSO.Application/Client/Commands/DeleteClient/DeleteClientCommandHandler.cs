namespace SSO.Application.Client.Commands.DeleteClient
{
    using Domain.EntityFramework;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, Unit>
    {
        private readonly AuthDbContext _authContext;

        public DeleteClientCommandHandler(AuthDbContext authContext)
        {
            _authContext = authContext;
        }

        public async Task<Unit> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
        {
            var client = await _authContext.Clients.FindAsync(request.Id);

            _authContext.Clients.Remove(client);

            await _authContext.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
