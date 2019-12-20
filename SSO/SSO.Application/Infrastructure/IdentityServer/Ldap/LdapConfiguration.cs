namespace SSO.Application.Infrastructure.IdentityServer.Ldap
{
    public class LdapConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Dn { get; set; }
        public string Password { get; set; }
    }
}
