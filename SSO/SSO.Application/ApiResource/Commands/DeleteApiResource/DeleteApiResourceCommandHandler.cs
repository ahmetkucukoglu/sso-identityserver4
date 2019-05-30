namespace SSO.Application.ApiResource.Commands.DeleteApiResource
{
    using Domain.EntityFramework;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteApiResourceCommandHandler : IRequestHandler<DeleteApiResourceCommand, Unit>
    {
        private readonly AuthDbContext _authContext;

        public DeleteApiResourceCommandHandler(AuthDbContext authContext)
        {
            _authContext = authContext;
        }

        public async Task<Unit> Handle(DeleteApiResourceCommand request, CancellationToken cancellationToken)
        {
            var apiResource = await _authContext.ApiResources.FindAsync(request.Name);

            _authContext.ApiResources.Remove(apiResource);

            await _authContext.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
