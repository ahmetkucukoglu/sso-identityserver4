namespace SSO.Server.Controllers
{
    using Application.Grant.Commands.Revoke;
    using Application.Grant.Queries.GetGrantList;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Authorize(Policy = "Admin")]
    public class GrantController : Controller
    {
        private readonly IMediator _mediator;

        public GrantController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var grants = await _mediator.Send(new GetGrantListQuery());

            return View(grants);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Revoke(string clientId)
        {
            await _mediator.Send(new RevokeCommand { ClientId = clientId });

            return RedirectToAction("Index");
        }
    }
}