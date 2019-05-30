namespace SSO.Application.IdentityResource.Queries.GetIdentityResourceEditViewModel
{
    using Commands.UpdateIdentityResource;
    using System.Collections.Generic;

    public class IdentityResourceEditViewModel
    {
        public IdentityResourceEditViewModel()
        {
            Command = new UpdateIdentityResourceCommand();
            Claims = new List<string>();
        }

        public UpdateIdentityResourceCommand Command { get; set; }
        public IEnumerable<string> Claims { get; set; }
    }
}
