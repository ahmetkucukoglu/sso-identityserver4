﻿namespace SSO.Application.Client.Commands.UpdateClient
{
    using Domain.EntityFramework;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, Unit>
    {
        private readonly AuthDbContext _authContext;

        public UpdateClientCommandHandler(AuthDbContext authContext)
        {
            _authContext = authContext;
        }

        public async Task<Unit> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            var client = await _authContext
                    .Clients
                    .Include((x) => x.IdentityResources)
                    .Include((x) => x.ApiResources)
                    .FirstOrDefaultAsync((x) => x.Id == request.Id);

            client.Name = request.Name;
            client.PostLogoutRedirectUri = request.Type == 1 ? request.PostLogoutRedirectUri : null;
            client.RedirectUri = request.Type == 1 ? request.RedirectUri : null;
            client.RequireConsent = request.Type == 1 ? request.RequireConsent : false;
            client.AllowedGrantTypes = request.AllowedGrantTypes;
            client.Enabled = request.Enabled;
            
            client.IdentityResources.Clear();

            if (request.Type == 1)
            {
                foreach (var identityResource in request.SelectedIdentityResources)
                {
                    client.IdentityResources.Add(new Domain.Entities.ClientIdentityResource { IdentityResourceId = identityResource });
                }
            }

            client.ApiResources.Clear();

            if (request.Type == 2)
            {
                foreach (var apiResource in request.SelectedApiResources)
                {
                    client.ApiResources.Add(new Domain.Entities.ClientApiResource { ApiResourceId = apiResource });
                }
            }

            await _authContext.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
