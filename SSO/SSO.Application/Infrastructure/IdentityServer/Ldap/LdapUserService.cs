namespace SSO.Application.User.Services.Ldap
{
    using Application.Account.Commands.Login;
    using Application.Exceptions;
    using Application.Infrastructure.IdentityServer;
    using Application.User.Commands.ChangePassword;
    using Application.User.Commands.CreateUser;
    using Application.User.Commands.DeleteUser;
    using Application.User.Commands.ForgotPassword;
    using Application.User.Commands.ResetPassword;
    using Application.User.Commands.UpdateUser;
    using Application.User.Queries.GetUserDetail;
    using Application.User.Queries.GetUserList;
    using IdentityServer4.Events;
    using IdentityServer4.Services;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class LdapUserService : IIdentityUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly IEventService _eventService;
        private readonly LdapUserStore _ldapUserStore;

        public LdapUserService(
            IHttpContextAccessor httpContextAccessor,
            IIdentityServerInteractionService interactionService,
            IEventService eventService,
            LdapUserStore ldapUserStore)
        {
            _httpContextAccessor = httpContextAccessor;
            _interactionService = interactionService;
            _eventService = eventService;
            _ldapUserStore = ldapUserStore;
        }

        public Task ChangePassword(ChangePasswordCommand request)
        {
            throw new NotImplementedException();
        }

        public Task CreateUser(CreateUserCommand request)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUser(DeleteUserCommand request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ForgotPassword(ForgotPasswordCommand request)
        {
            throw new NotImplementedException();
        }

        public Task<UserDetail> GetUserDetail(GetUserDetailQuery request)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserListItem>> GetUserList(GetUserListQuery request)
        {
            return Task.FromResult(_ldapUserStore.GetUsers());
        }

        public async Task<string> Login(LoginCommand request)
        {
            string returnUrl = string.Empty;

            try
            {
                _ldapUserStore.Login(request.Username, request.Password);

                AuthenticationProperties props = null;

                var claims = _ldapUserStore.GetClaims(request.Username);

                await _httpContextAccessor.HttpContext.SignInAsync(request.Username, request.Username, props, claims.ToArray());

                await _eventService.RaiseAsync(new UserLoginSuccessEvent(request.Username, request.Username, request.Username));

                if (_interactionService.IsValidReturnUrl(request.ReturnUrl))
                {
                    returnUrl = request.ReturnUrl;
                }
                else
                {
                    returnUrl = "~/";
                }
            }
            catch
            {
                await _eventService.RaiseAsync(new UserLoginFailureEvent(request.Username, "invalid credentials"));

                throw new UserFriendlyException("Invalid username or password");
            }

            return returnUrl;
        }

        public Task ResetPassword(ResetPasswordCommand request)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUser(UpdateUserCommand request)
        {
            throw new NotImplementedException();
        }
    }
}
