namespace SSO.Application.IdentityResource.Queries.GetIdentityResourceCreateViewModel
{
    using Commands.CreateIdentityResource;
    using System.Collections.Generic;

    public class IdentityResourceCreateViewModel
    {
        public IdentityResourceCreateViewModel()
        {
            Command = new CreateIdentityResourceCommand();
            Claims = new List<string>();
        }

        public CreateIdentityResourceCommand Command { get; set; }
        public IEnumerable<string> Claims { get; set; }
    }
}
