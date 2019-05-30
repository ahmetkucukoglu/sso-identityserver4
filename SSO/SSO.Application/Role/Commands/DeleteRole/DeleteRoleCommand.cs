namespace SSO.Application.Role.Commands.DeleteRole
{
    using MediatR;

    public class DeleteRoleCommand : IRequest
    {
        public string Name { get; set; }
    }
}
