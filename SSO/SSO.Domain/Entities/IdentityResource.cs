namespace SSO.Domain.Entities
{
    using System.Collections.Generic;

    public class IdentityResource
    {
        public IdentityResource()
        {
            Claims = new List<IdentityResourceClaim>();
        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Enabled { get; set; }
        public ICollection<IdentityResourceClaim> Claims { get; set; }
    }
}
