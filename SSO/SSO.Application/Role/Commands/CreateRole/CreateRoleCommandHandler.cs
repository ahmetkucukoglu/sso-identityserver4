namespace SSO.Application.Role.Commands.CreateRole
{
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Unit>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public CreateRoleCommandHandler(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<Unit> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            await _roleManager.CreateAsync(new IdentityRole(request.Name));

            return Unit.Value;
        }
    }
}
