namespace SSO.Server.Controllers
{
    using Application.Client.Commands.CreateClient;
    using Application.Client.Commands.DeleteClient;
    using Application.Client.Commands.ReGenerateSecret;
    using Application.Client.Commands.UpdateClient;
    using Application.Client.Commands.UploadLogo;
    using Application.Client.Queries.GetClientCreateViewModel;
    using Application.Client.Queries.GetClientEditViewModel;
    using Application.Client.Queries.GetClientList;
    using Application.Infrastructure.AspNet.Extensions;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Authorize(Roles = "auth.admin")]
    public class ClientController : Controller
    {
        private readonly IMediator _mediator;

        public ClientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var clients = await _mediator.Send(new GetClientListQuery());

            return View(clients);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = await _mediator.Send(new ClientCreateViewModelQuery());

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClientCommand command)
        {
            try
            {
                await _mediator.Send(command);

                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ModelState.AddModelErrorIfUserFriendlyException(exception);

                var viewModel = await _mediator.Send(new ClientCreateViewModelQuery { Command = command });

                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var viewModel = await _mediator.Send(new ClientEditViewModelQuery { Id = id });

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateClientCommand command)
        {
            try
            {
                await _mediator.Send(command);

                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ModelState.AddModelErrorIfUserFriendlyException(exception);

                var viewModel = await _mediator.Send(new ClientEditViewModelQuery { Command = command });

                return View(viewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ReGenerateSecret(ReGenerateSecretCommand command)
        {
            try
            {
                await _mediator.Send(command);

                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                TempData.AddTempDataIfUserFriendlyException(exception);

                return RedirectToAction("Edit", "Client", new { id = command.Id }, "resecret");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadLogo(UploadLogoCommand command)
        {
            try
            {
                await _mediator.Send(command);

                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                TempData.AddTempDataIfUserFriendlyException(exception);

                return RedirectToAction("Edit", "Client", new { id = command.Id }, "uploadlogo");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteClientCommand { Id = id });

            return RedirectToAction("Index");
        }
    }
}