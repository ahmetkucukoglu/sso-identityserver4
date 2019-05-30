namespace SSO.Domain.Entities
{
    public class ClientIdentityResource
    {
        public string ClientId { get; set; }
        public Client Client { get; set; }
        public string IdentityResourceId { get; set; }
        public IdentityResource IdentityResource { get; set; }
    }
}
