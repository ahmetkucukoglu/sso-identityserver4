namespace SSO.Infrastructure.Cache
{
    public class RedisCacheSettings
    {
        public string Master { get; set; }
        public string Slave { get; set; }
        public string InstanceName { get; set; }
    }
}
