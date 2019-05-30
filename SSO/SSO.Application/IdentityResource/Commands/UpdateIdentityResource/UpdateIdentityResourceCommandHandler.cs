namespace SSO.Application.IdentityResource.Commands.UpdateIdentityResource
{
    using Domain.EntityFramework;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateIdentityResourceCommandHandler : IRequestHandler<UpdateIdentityResourceCommand, Unit>
    {
        private readonly AuthDbContext _authContext;

        public UpdateIdentityResourceCommandHandler(AuthDbContext authContext)
        {
            _authContext = authContext;
        }

        public async Task<Unit> Handle(UpdateIdentityResourceCommand request, CancellationToken cancellationToken)
        {
            var identityResource = await _authContext
                    .IdentityResources
                    .Include((x) => x.Claims)
                    .ThenInclude((x) => x.Claim)
                    .FirstOrDefaultAsync((x) => x.Name == request.Name);
        
            identityResource.Description = request.Description;
            identityResource.DisplayName = request.DisplayName;
            identityResource.Enabled = request.Enabled;
            identityResource.Required = request.Required;

            identityResource.Claims.Clear();

            foreach (var claim in request.SelectedClaims)
            {
                identityResource.Claims.Add(new Domain.Entities.IdentityResourceClaim { ClaimId = claim });
            }

            await _authContext.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
