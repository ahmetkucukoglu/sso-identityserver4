namespace SSO.Application.Infrastructure.IdentityServer
{
    using Application.Account.Commands.Login;
    using Application.Exceptions;
    using Application.User.Commands.ChangePassword;
    using Domain.Entities;
    using IdentityModel;
    using IdentityServer4.Events;
    using IdentityServer4.Models;
    using IdentityServer4.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using SSO.Infrastructure.Email;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using User.Commands.CreateUser;
    using User.Commands.DeleteUser;
    using User.Commands.ForgotPassword;
    using User.Commands.ResetPassword;
    using User.Commands.UpdateUser;
    using User.Queries.GetUserDetail;
    using User.Queries.GetUserList;

    public class IdentityUserService : IIdentityUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly IEventService _eventService;

        public IdentityUserService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IHttpContextAccessor httpContextAccessor,
            IEmailService emailService,
            IIdentityServerInteractionService interactionService,
            IEventService eventService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _interactionService = interactionService;
            _eventService = eventService;
        }

        public async Task ChangePassword(ChangePasswordCommand request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);

            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.Password);

            if (!result.Succeeded)
                throw new UserFriendlyException(result.Errors.Select((x) => $"{x.Code}:{x.Description}").ToList());
        }

        public async Task CreateUser(CreateUserCommand request)
        {
            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
                EmailConfirmed = request.EmailVerified
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                throw new UserFriendlyException(result.Errors.Select((x) => $"{x.Code}:{x.Description}").ToList());

            var claims = new List<System.Security.Claims.Claim> {
                new System.Security.Claims.Claim(JwtClaimTypes.Name, $"{request.GivenName} {request.FamilyName}"),
                new System.Security.Claims.Claim(JwtClaimTypes.GivenName, request.GivenName),
                new System.Security.Claims.Claim(JwtClaimTypes.FamilyName, request.FamilyName)
            };

            result = await _userManager.AddClaimsAsync(user, claims);

            if (!result.Succeeded)
                throw new UserFriendlyException(result.Errors.Select((x) => $"{x.Code}:{x.Description}").ToList());

            foreach (var item in request.SelectedRoles)
            {
                await _userManager.AddToRoleAsync(user, item);
            }
        }

        public async Task DeleteUser(DeleteUserCommand request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                throw new UserFriendlyException(result.Errors.Select((x) => $"{x.Code}:{x.Description}").ToList());
        }

        public async Task<bool> ForgotPassword(ForgotPasswordCommand request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                throw new UserFriendlyException("Invalid email");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var uri = new UriBuilder(_httpContextAccessor.HttpContext.Request.Scheme, _httpContextAccessor.HttpContext.Request.Host.Host, _httpContextAccessor.HttpContext.Request.Host.Port.GetValueOrDefault(80), "/Account/ResetPassword");

            uri.Query = $"token={WebUtility.UrlEncode(token)}";

            var claims = await _userManager.GetClaimsAsync(user);
            var name = claims.FirstOrDefault((x) => x.Type == JwtClaimTypes.GivenName)?.Value;

            var result = await _emailService.SendResetPasswordLink(user.Email, name, uri.ToString());

            return result;
        }

        public async Task ResetPassword(ResetPasswordCommand request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                throw new UserFriendlyException("Invalid email");

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

            if (!result.Succeeded)
                throw new UserFriendlyException("Invalid token");
        }

        public async Task UpdateUser(UpdateUserCommand request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            var claims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            user.UserName = request.UserName;
            user.Email = request.Email;
            user.EmailConfirmed = request.EmailVerified;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new UserFriendlyException(result.Errors.Select((x) => $"{x.Code}:{x.Description}").ToList());

            result = await _userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
                throw new UserFriendlyException(result.Errors.Select((x) => $"{x.Code}:{x.Description}").ToList());

            var newClaims = new List<System.Security.Claims.Claim> {
                    new System.Security.Claims.Claim(JwtClaimTypes.Name, $"{request.GivenName} {request.FamilyName}"),
                    new System.Security.Claims.Claim(JwtClaimTypes.GivenName, request.GivenName),
                    new System.Security.Claims.Claim(JwtClaimTypes.FamilyName, request.FamilyName)
                };

            result = await _userManager.AddClaimsAsync(user, newClaims);

            if (!result.Succeeded)
                throw new UserFriendlyException(result.Errors.Select((x) => $"{x.Code}:{x.Description}").ToList());

            var addedRoles = request.SelectedRoles.Except(roles);
            var removedRoles = roles.Except(request.SelectedRoles);

            foreach (var item in addedRoles)
            {
                await _userManager.AddToRoleAsync(user, item);
            }

            foreach (var item in removedRoles)
            {
                await _userManager.RemoveFromRoleAsync(user, item);
            }
        }

        public async Task<UserDetail> GetUserDetail(GetUserDetailQuery request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            var claims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            return new UserDetail
            {
                Id = user.Id,
                UserName = user.UserName,
                GivenName = claims.FirstOrDefault((x) => x.Type == JwtClaimTypes.GivenName)?.Value,
                FamilyName = claims.FirstOrDefault((x) => x.Type == JwtClaimTypes.FamilyName)?.Value,
                Email = user.Email,
                SelectedRoles = roles,
                EmailVerified = user.EmailConfirmed
            };
        }

        public async Task<IEnumerable<UserListItem>> GetUserList(GetUserListQuery request)
        {
            var result = new List<UserListItem>();

            var users = _userManager.Users.ToList();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                result.Add(new UserListItem
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles.Aggregate((x, y) => x + "," + y)
                });
            }

            return result;
        }

        public async Task<string> Login(LoginCommand request)
        {
            string returnUrl = string.Empty;

            if (request.Action != "login")
            {
                var context = await _interactionService.GetAuthorizationContextAsync(request.ReturnUrl);

                if (context != null)
                {
                    await _interactionService.GrantConsentAsync(context, ConsentResponse.Denied);

                    returnUrl = request.ReturnUrl;
                }
                else
                {
                    returnUrl = "~/";
                }
            }
            else
            {
                var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, request.RememberLogin, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(request.Username);

                    await _eventService.RaiseAsync(new UserLoginSuccessEvent(request.Username, user.Id, request.Username));

                    if (_interactionService.IsValidReturnUrl(request.ReturnUrl))
                    {
                        returnUrl = request.ReturnUrl;
                    }
                    else
                    {
                        returnUrl = "~/";
                    }
                }
                else
                {
                    await _eventService.RaiseAsync(new UserLoginFailureEvent(request.Username, "invalid credentials"));

                    throw new UserFriendlyException("Invalid username or password");
                }
            }

            return returnUrl;
        }
    }
}
