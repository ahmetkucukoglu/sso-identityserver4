namespace SSO.Application.User.Commands.DeleteUser
{
    using Application.Exceptions;
    using Domain.Entities;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteUserCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                throw new UserFriendlyException(result.Errors.Select((x) => $"{x.Code}:{x.Description}").ToList());

            return Unit.Value;
        }
    }
}
