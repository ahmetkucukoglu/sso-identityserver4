namespace SSO.Application.IdentityResource.Queries.GetIdentityResourceDetail
{
    using Commands.UpdateIdentityResource;
    using System.Collections.Generic;

    public class IdentityResourceDetail
    {
        public IdentityResourceDetail()
        {
            SelectedClaims = new List<string>();
        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public bool Required { get; set; }
        public IEnumerable<string> SelectedClaims { get; set; }

        public static explicit operator UpdateIdentityResourceCommand(IdentityResourceDetail identityResourceDetail)
        {
            return new UpdateIdentityResourceCommand
            {
                Description = identityResourceDetail.Description,
                DisplayName = identityResourceDetail.DisplayName,
                Enabled = identityResourceDetail.Enabled,
                Name = identityResourceDetail.Name,
                Required = identityResourceDetail.Required,
                SelectedClaims = identityResourceDetail.SelectedClaims
            };
        }
    }
}
