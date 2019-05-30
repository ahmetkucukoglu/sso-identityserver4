namespace SSO.Server.Controllers
{
    using Application.Infrastructure.AspNet.Extensions;
    using Application.User.Commands.ChangePassword;
    using Application.User.Commands.CreateUser;
    using Application.User.Commands.DeleteUser;
    using Application.User.Commands.UpdateUser;
    using Application.User.Queries.GetUserCreateViewModel;
    using Application.User.Queries.GetUserEditViewModel;
    using Application.User.Queries.GetUserList;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Authorize(Roles = "auth.admin")]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _mediator.Send(new GetUserListQuery());

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = await _mediator.Send(new UserCreateViewModelQuery());

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserCommand command)
        {
            try
            {
                await _mediator.Send(command);

                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ModelState.AddModelErrorIfUserFriendlyException(exception);

                var viewModel = await _mediator.Send(new UserCreateViewModelQuery { Command = command });

                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var viewModel = await _mediator.Send(new UserEditViewModelQuery { Id = id });

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateUserCommand command)
        {
            try
            {
                await _mediator.Send(command);

                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ModelState.AddModelErrorIfUserFriendlyException(exception);

                var viewModel = await _mediator.Send(new UserEditViewModelQuery { Command = command });

                return View(viewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
        {
            try
            {
                await _mediator.Send(command);

                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                TempData.AddTempDataIfUserFriendlyException(exception);

                return RedirectToAction("Edit", "User", new { id = command.Id }, "password");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteUserCommand { Id = id });

            return RedirectToAction("Index");
        }
    }
}