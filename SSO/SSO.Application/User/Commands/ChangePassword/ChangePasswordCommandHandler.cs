namespace SSO.Application.User.Commands.ChangePassword
{
    using Domain.Entities;
    using Exceptions;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ChangePasswordCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);

            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.Password);

            if (!result.Succeeded)
                throw new UserFriendlyException(result.Errors.Select((x) => $"{x.Code}:{x.Description}").ToList());

            return Unit.Value;
        }
    }
}
