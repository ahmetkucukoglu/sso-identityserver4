namespace SSO.Server.Controllers
{
    using Application.Infrastructure.AspNet.Extensions;
    using Application.Role.Commands.CreateRole;
    using Application.Role.Commands.DeleteRole;
    using Application.Role.Queries.GetRoleList;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Authorize(Roles = "auth.admin")]
    public class RoleController : Controller
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roles = await _mediator.Send(new GetRoleListQuery());

            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleCommand command)
        {
            try
            {
                await _mediator.Send(command);

                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ModelState.AddModelErrorIfUserFriendlyException(exception);

                return View(command);
            }            
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string name)
        {
            await _mediator.Send(new DeleteRoleCommand { Name = name });

            return RedirectToAction("Index");
        }
    }
}