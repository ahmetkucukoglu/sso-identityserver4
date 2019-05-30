namespace SSO.Application.IdentityResource.Queries.GetIdentityResourceList
{
    public class IdentityResourceListItem
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Enabled { get; set; }
    }
}
