namespace SSO.Application.Client.Queries.GetClientList
{
    public class ClientListItem
    {
        public string Id { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public bool RequireConsent { get; set; }
        public string LogoUri { get; set; }
    }
}
