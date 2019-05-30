namespace SSO.Application.IdentityResource.Commands.UpdateIdentityResource
{
    using MediatR;
    using System.Collections.Generic;

    public class UpdateIdentityResourceCommand : IRequest
    {
        public UpdateIdentityResourceCommand()
        {
            SelectedClaims = new List<string>();
        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public bool Required { get; set; }
        public IEnumerable<string> SelectedClaims { get; set; }
    }
}
