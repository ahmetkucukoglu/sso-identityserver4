namespace SSO.Application.User.Commands.ChangePassword
{
    using MediatR;

    public class ChangePasswordCommand : IRequest
    {
        public string Id { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
    }
}
