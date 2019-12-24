namespace SSO.Application.User.Services.Ldap
{
    using Application.Infrastructure.IdentityServer.Ldap;
    using Application.User.Queries.GetUserList;
    using IdentityModel;
    using Novell.Directory.Ldap;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    public class LdapUserStore : IDisposable
    {
        private LdapConnection _ldapConnection;
        private List<LdapUser> _ldapUsers = new List<LdapUser>();
        private Dictionary<string, string> _distinguishedNames = new Dictionary<string, string>();

        public LdapUserStore(LdapConfiguration ldapConfiguration)
        {
            _ldapConnection = new LdapConnection();

            var searchConstraints = _ldapConnection.SearchConstraints;
            searchConstraints.ReferralFollowing = true;

            _ldapConnection.Constraints = searchConstraints;

            _ldapConnection.Connect(ldapConfiguration.Host, ldapConfiguration.Port);

            _ldapConnection.Bind(LdapConnection.Ldap_V3, ldapConfiguration.Dn, ldapConfiguration.Password);

            var searchResults = _ldapConnection.Search(
                "DC=medyanet,DC=local",
                LdapConnection.SCOPE_SUB,
                "(&(objectClass=user)(objectCategory=person))",
                new string[] { "sn", "mail", "memberOf", "givenName", "samaccountname", "distinguishedname", "userAccountControl" },
                false);

            while (searchResults.hasMore())
            {
                var ldapEntry = searchResults.next();

                var sn = ldapEntry.getAttribute("sn");
                var mail = ldapEntry.getAttribute("mail");
                var memberOf = ldapEntry.getAttribute("memberOf");
                var givenName = ldapEntry.getAttribute("givenName");
                var samaccountName = ldapEntry.getAttribute("samaccountname");
                var distinguishedName = ldapEntry.getAttribute("distinguishedname");
                var userAccountControl = ldapEntry.getAttribute("userAccountControl");

                if (samaccountName != null && memberOf != null && mail != null)
                {
                    _distinguishedNames.Add(samaccountName?.StringValue, distinguishedName?.StringValue);

                    string roles = string.Empty;

                    foreach (var memberItem in memberOf.StringValueArray)
                    {
                        var splitted = memberItem.Split(",");

                        foreach (var item in splitted)
                        {
                            if (item.StartsWith("CN"))
                            {
                                if (!string.IsNullOrEmpty(roles))
                                    roles += ",";

                                roles += item.Replace("CN=", "");
                            }
                        }
                    }

                    var user = new LdapUser
                    {
                        Email = mail?.StringValue,
                        UserName = samaccountName?.StringValue,
                        FirstName = givenName?.StringValue,
                        LastName = sn?.StringValue,
                        Roles = roles,
                        IsActive = !Convert.ToBoolean(int.Parse(userAccountControl.StringValue) & 0x0002)
                    };

                    _ldapUsers.Add(user);
                }
            }
        }

        public void Login(string username, string password)
        {
            var dn = _distinguishedNames[username];

            _ldapConnection.Bind(dn, password);
        }

        public IEnumerable<UserListItem> GetUsers()
        {
            return _ldapUsers.Select((x) => new UserListItem
            {
                Id = x.UserName,
                Email = x.Email,
                UserName = x.UserName,
                Roles = x.Roles
            });
        }

        public UserListItem GetUser(string username)
        {
            var user = _ldapUsers.FirstOrDefault((x) => x.UserName == username);

            if (user == null)
                return null;

            return new UserListItem
            {
                Id = user.UserName,
                Email = user.Email,
                UserName = user.UserName,
                Roles = user.Roles
            };
        }

        public List<Claim> GetClaims(string username)
        {
            var user = _ldapUsers.FirstOrDefault((x) => x.UserName == username);

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(JwtClaimTypes.GivenName, user.FirstName),
                new Claim(JwtClaimTypes.FamilyName, user.LastName),
                new Claim(JwtClaimTypes.Email, user.Email),
                new Claim(JwtClaimTypes.EmailVerified, bool.TrueString)
            };

            var roles = user.Roles.Split(",");

            foreach (var item in roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, item));
            }

            return claims;
        }

        public void Dispose()
        {
            _ldapConnection.Disconnect();
        }
    }
}
