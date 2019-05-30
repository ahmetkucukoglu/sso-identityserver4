namespace SSO.Server.Controllers
{
    using Application.IdentityResource.Commands.CreateIdentityResource;
    using Application.IdentityResource.Commands.DeleteIdentityResource;
    using Application.IdentityResource.Commands.UpdateIdentityResource;
    using Application.IdentityResource.Queries.GetIdentityResourceCreateViewModel;
    using Application.IdentityResource.Queries.GetIdentityResourceEditViewModel;
    using Application.IdentityResource.Queries.GetIdentityResourceList;
    using Application.Infrastructure.AspNet.Extensions;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Authorize(Roles = "auth.admin")]
    public class IdentityResourceController : Controller
    {
        private readonly IMediator _mediator;

        public IdentityResourceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var identityResources = await _mediator.Send(new GetIdentityResourceListQuery());

            return View(identityResources);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = await _mediator.Send(new IdentityResourceCreateViewModelQuery());

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateIdentityResourceCommand command)
        {
            try
            {
                await _mediator.Send(command);

                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ModelState.AddModelErrorIfUserFriendlyException(exception);

                var viewModel = await _mediator.Send(new IdentityResourceCreateViewModelQuery { Command = command });

                return View(viewModel);
            }
        }

        public async Task<IActionResult> Edit(string name)
        {
            var viewModel = await _mediator.Send(new IdentityResourceEditViewModelQuery { Name = name });

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateIdentityResourceCommand command)
        {
            try
            {
                await _mediator.Send(command);

                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ModelState.AddModelErrorIfUserFriendlyException(exception);

                var viewModel = await _mediator.Send(new IdentityResourceEditViewModelQuery { Command = command });

                return View(viewModel);
            }
        }

        public async Task<IActionResult> Delete(string name)
        {
            await _mediator.Send(new DeleteIdentityResourceCommand { Name = name });

            return RedirectToAction("Index");
        }
    }
}