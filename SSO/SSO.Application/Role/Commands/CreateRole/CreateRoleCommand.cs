namespace SSO.Application.Role.Commands.CreateRole
{
    using MediatR;

    public class CreateRoleCommand : IRequest
    {
        public string Name { get; set; }
    }
}
