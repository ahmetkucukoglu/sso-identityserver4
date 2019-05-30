namespace SSO.Application.ApiResource.Commands.CreateApiResource
{
    using Domain.EntityFramework;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateApiResourceCommandHandler : IRequestHandler<CreateApiResourceCommand, Unit>
    {
        private readonly AuthDbContext _authContext;

        public CreateApiResourceCommandHandler(AuthDbContext authContext)
        {
            _authContext = authContext;
        }

        public async Task<Unit> Handle(CreateApiResourceCommand request, CancellationToken cancellationToken)
        {
            var apiResource = new Domain.Entities.ApiResource
            {
                Name = request.Name,
                Description = request.Description,
                DisplayName = request.DisplayName,
                Enabled = request.Enabled
            };

            foreach (var selectedClaim in request.SelectedClaims)
            {
                apiResource.Claims.Add(new Domain.Entities.ApiResourceClaim { ClaimId = selectedClaim });
            }

            await _authContext.ApiResources.AddAsync(apiResource);

            await _authContext.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
