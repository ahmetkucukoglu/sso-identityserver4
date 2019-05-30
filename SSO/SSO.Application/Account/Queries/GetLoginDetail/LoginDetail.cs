namespace SSO.Application.Account.Queries.GetLoginDetail
{
    public class LoginDetail
    {
        public bool AllowRememberLogin { get; set; }
        public bool EnableLocalLogin { get; set; }
        public string ReturnUrl { get; set; }
        public string UserName { get; set; }
    }
}
