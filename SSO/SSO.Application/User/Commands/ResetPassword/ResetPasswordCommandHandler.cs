namespace SSO.Application.User.Commands.ResetPassword
{
    using SSO.Infrastructure.Email;
    using Domain.Entities;
    using Exceptions;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using System.Threading;
    using System.Threading.Tasks;

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Unit>
    {
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordCommandHandler(
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IEmailService emailService)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                throw new UserFriendlyException("Invalid email");

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

            if (!result.Succeeded)
                throw new UserFriendlyException("Invalid token");

            return Unit.Value;
        }
    }
}
