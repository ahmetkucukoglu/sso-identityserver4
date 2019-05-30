namespace SSO.Server.Controllers
{
    using Application.ApiResource.Commands.CreateApiResource;
    using Application.ApiResource.Commands.DeleteApiResource;
    using Application.ApiResource.Commands.UpdateApiResource;
    using Application.ApiResource.Queries.GetApiResourceCreateViewModel;
    using Application.ApiResource.Queries.GetApiResourceEditViewModel;
    using Application.ApiResource.Queries.GetApiResourceList;
    using Application.Infrastructure.AspNet.Extensions;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Authorize(Roles = "auth.admin")]
    public class ApiResourceController : Controller
    {
        private readonly IMediator _mediator;

        public ApiResourceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var apiResources = await _mediator.Send(new GetApiResourceListQuery());

            return View(apiResources);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = await _mediator.Send(new ApiResourceCreateViewModelQuery());

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateApiResourceCommand command)
        {
            try
            {
                await _mediator.Send(command);

                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ModelState.AddModelErrorIfUserFriendlyException(exception);

                var viewModel = await _mediator.Send(new ApiResourceCreateViewModelQuery { Command = command });

                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string name)
        {
            var viewModel = await _mediator.Send(new ApiResourceEditViewModelQuery { Name = name });

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateApiResourceCommand command)
        {
            try
            {
                await _mediator.Send(command);

                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ModelState.AddModelErrorIfUserFriendlyException(exception);

                var viewModel = await _mediator.Send(new ApiResourceEditViewModelQuery { Command = command });

                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string name)
        {
            await _mediator.Send(new DeleteApiResourceCommand { Name = name });

            return RedirectToAction("Index");
        }
    }
}