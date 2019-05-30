namespace SSO.Application.User.Commands.DeleteUser
{
    using MediatR;

    public class DeleteUserCommand : IRequest
    {
        public string Id { get; set; }
    }
}
