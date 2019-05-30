namespace SSO.Application.IdentityResource.Commands.CreateIdentityResource
{
    using MediatR;
    using System.Collections.Generic;

    public class CreateIdentityResourceCommand : IRequest
    {
        public CreateIdentityResourceCommand()
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
