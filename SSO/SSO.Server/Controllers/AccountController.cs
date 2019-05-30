namespace SSO.Server.Controllers
{
    using Application.Account.Commands.Login;
    using Application.Account.Commands.Logout;
    using Application.Account.Queries.GetLoginDetail;
    using Application.Infrastructure.AspNet.Extensions;
    using Application.User.Commands.ForgotPassword;
    using Application.User.Commands.ResetPassword;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    public class AccountController : Controller
    {
        private readonly IMediator _mediator;
        
        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var loginDetail = await _mediator.Send(new GetLoginDetailQuery { ReturnUrl = returnUrl });

            var command = new LoginCommand
            {
                AllowRememberLogin = loginDetail.AllowRememberLogin,
                EnableLocalLogin = loginDetail.EnableLocalLogin
            };
            
            return View(command);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            try
            {
                var returnUrl = await _mediator.Send(command);

                return Redirect(returnUrl);
            }
            catch (Exception exception)
            {
                ModelState.AddModelErrorIfUserFriendlyException(exception);

                return View(command);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var output = await _mediator.Send(new LogoutCommand { Id = logoutId });

            return View("LoggedOut", output);
        }
        
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand command)
        {
            var succeeded = false;

            try
            {
                succeeded = await _mediator.Send(command);

                if (succeeded)
                    return RedirectToAction("Login");
                else
                    ModelState.AddModelError("", "Not sending email.");
            }
            catch (Exception exception)
            {
                ModelState.AddModelErrorIfUserFriendlyException(exception);
            }

            return View(command);
        }

        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            try
            {
                await _mediator.Send(command);

                return RedirectToAction("Login");
            }
            catch (Exception exception)
            {
                ModelState.AddModelErrorIfUserFriendlyException(exception);

                return View(command);
            }
        }
    }
}