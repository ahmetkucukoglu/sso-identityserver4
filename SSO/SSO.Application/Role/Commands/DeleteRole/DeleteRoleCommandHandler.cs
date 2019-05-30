namespace SSO.Application.Role.Commands.DeleteRole
{
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Unit>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public DeleteRoleCommandHandler(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByNameAsync(request.Name);

            await _roleManager.DeleteAsync(role);

            return Unit.Value;
        }
    }
}
