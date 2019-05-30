namespace SSO.Application.User.Commands.ForgotPassword
{
    using MediatR;

    public class ForgotPasswordCommand : IRequest<bool>
    {
        public string Email { get; set; }
    }
}
