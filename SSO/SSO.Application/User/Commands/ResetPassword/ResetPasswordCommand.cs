namespace SSO.Application.User.Commands.ResetPassword
{
    using MediatR;

    public class ResetPasswordCommand : IRequest
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}
