namespace SSO.Application.User.Services.Ldap
{
    public class LdapUser
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
        public bool IsActive { get; set; }
    }
}
