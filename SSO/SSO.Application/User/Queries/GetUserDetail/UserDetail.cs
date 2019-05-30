namespace SSO.Application.User.Queries.GetUserDetail
{
    using System.Collections.Generic;

    public class UserDetail
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        public IEnumerable<string> SelectedRoles { get; set; }
    }
}
