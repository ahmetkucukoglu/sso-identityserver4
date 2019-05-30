namespace SSO.Server.Controllers
{
    using Application.Consent.Commands.ProccessConsent;
    using Application.Consent.Queries.GetConsentDetail;
    using Application.Infrastructure.AspNet.Extensions;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    public class ConsentController : Controller
    {
        private readonly IMediator _mediator;

        public ConsentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            try
            {
                var detail = await _mediator.Send(new GetConsentDetailQuery { ReturnUrl = returnUrl });

                return View(detail);
            }
            catch
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(ProccessConsentCommand command)
        {
            try
            {
                await _mediator.Send(command);

                return Redirect(command.ReturnUrl);
            }
            catch (Exception exception)
            {
                ModelState.AddModelErrorIfUserFriendlyException(exception);

                return RedirectToAction("Index", new { returnUrl = command.ReturnUrl });
            }
        }
    }
}
