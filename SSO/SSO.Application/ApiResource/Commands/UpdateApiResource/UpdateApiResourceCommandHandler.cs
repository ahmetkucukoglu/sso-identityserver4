namespace SSO.Application.ApiResource.Commands.UpdateApiResource
{
    using Domain.EntityFramework;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateApiResourceCommandHandler : IRequestHandler<UpdateApiResourceCommand, Unit>
    {
        private readonly AuthDbContext _authContext;

        public UpdateApiResourceCommandHandler(AuthDbContext authContext)
        {
            _authContext = authContext;
        }

        public async Task<Unit> Handle(UpdateApiResourceCommand request, CancellationToken cancellationToken)
        {
            var apiResource = await _authContext
               .ApiResources
               .Include((x) => x.Claims)
               .ThenInclude((x) => x.Claim)
               .FirstOrDefaultAsync((x) => x.Name == request.Name);

            apiResource.Description = request.Description;
            apiResource.DisplayName = request.DisplayName;
            apiResource.Enabled = request.Enabled;

            apiResource.Claims.Clear();

            foreach (var claim in request.SelectedClaims)
            {
                apiResource.Claims.Add(new Domain.Entities.ApiResourceClaim { ClaimId = claim });
            }

            await _authContext.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
