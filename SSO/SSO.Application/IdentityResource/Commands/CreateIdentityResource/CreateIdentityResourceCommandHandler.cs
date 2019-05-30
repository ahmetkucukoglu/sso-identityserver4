namespace SSO.Application.IdentityResource.Commands.CreateIdentityResource
{
    using Domain.EntityFramework;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateIdentityResourceCommandHandler : IRequestHandler<CreateIdentityResourceCommand, Unit>
    {
        private readonly AuthDbContext _authContext;

        public CreateIdentityResourceCommandHandler(AuthDbContext authContext)
        {
            _authContext = authContext;
        }

        public async Task<Unit> Handle(CreateIdentityResourceCommand request, CancellationToken cancellationToken)
        {
            var identityResource = new Domain.Entities.IdentityResource
            {
                Name = request.Name,
                Description = request.Description,
                DisplayName = request.DisplayName,
                Enabled = request.Enabled,
                Required = request.Required
            };

            foreach (var selectedClaim in request.SelectedClaims)
            {
                identityResource.Claims.Add(new Domain.Entities.IdentityResourceClaim { ClaimId = selectedClaim });
            }

            await _authContext.IdentityResources.AddAsync(identityResource);

            await _authContext.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
