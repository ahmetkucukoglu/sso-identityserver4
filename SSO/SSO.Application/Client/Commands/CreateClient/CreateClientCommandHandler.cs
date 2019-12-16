namespace SSO.Application.Client.Commands.CreateClient
{
    using Domain.EntityFramework;
    using IdentityModel;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;
    using UploadLogo;

    public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Unit>
    {
        private readonly AuthDbContext _authContext;
        private readonly IMediator _mediator;

        public CreateClientCommandHandler(AuthDbContext authContext, IMediator mediator)
        {
            _authContext = authContext;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            var client = new Domain.Entities.Client
            {
                Id = request.Id + request.IdSuffix,
                Name = request.Name,
                ClientSecret = request.Secret.ToSha256(),
                PostLogoutRedirectUri = request.Type == 1 ? request.PostLogoutRedirectUri : null,
                RedirectUri = request.Type == 1 ? request.RedirectUri : null,
                AllowedCorsOrigin = request.Type == 1 ? request.AllowedCorsOrigin : null,
                RequireConsent = request.Type == 1 ? request.RequireConsent : false,
                AllowedGrantTypes = request.AllowedGrantTypes,
                Enabled = request.Enabled
            };

            if (request.Type == 1)
            {
                foreach (var identityResource in request.SelectedIdentityResources)
                {
                    client.IdentityResources.Add(new Domain.Entities.ClientIdentityResource { IdentityResourceId = identityResource });
                }
            }

            if (request.Type == 2)
            {
                foreach (var apiResource in request.SelectedApiResources)
                {
                    client.ApiResources.Add(new Domain.Entities.ClientApiResource { ApiResourceId = apiResource });
                }
            }

            await _authContext.Clients.AddAsync(client);

            await _authContext.SaveChangesAsync();

            await _mediator.Send(new UploadLogoCommand { Id = client.Id, LogoFile = request.LogoFile });

            return Unit.Value;
        }
    }
}
