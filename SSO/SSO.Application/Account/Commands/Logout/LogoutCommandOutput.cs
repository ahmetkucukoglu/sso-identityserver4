namespace SSO.Application.Account.Commands.Logout
{
    public class LogoutCommandOutput
    {
        public string PostLogoutRedirectUri { get; set; }
        public string ClientName { get; set; }
        public string SignOutIframeUrl { get; set; }
        public bool AutomaticRedirectAfterSignOut { get; set; }
        public string LogoutId { get; set; }
    }
}
