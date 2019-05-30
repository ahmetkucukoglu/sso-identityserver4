namespace SSO.Application.Account.Commands.Login
{
    using MediatR;

    public class LoginCommand : IRequest<string>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
        public bool AllowRememberLogin { get; set; }
        public bool EnableLocalLogin { get; set; }
        public string Action { get; set; }
    }
}
