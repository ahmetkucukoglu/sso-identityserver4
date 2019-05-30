namespace SSO.Application.Account.Commands.Logout
{
    using MediatR;

    public class LogoutCommand : IRequest<LogoutCommandOutput>
    {
        public string Id { get; set; }
    }
}
