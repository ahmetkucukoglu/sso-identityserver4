namespace SSO.Domain.Entities
{
    public class ClientApiResource
    {
        public string ClientId { get; set; }
        public Client Client { get; set; }
        public string ApiResourceId { get; set; }
        public ApiResource ApiResource { get; set; }
    }
}
