namespace SSO.Server.Controllers
{
    using Application.Claim.Commands.CreateClaim;
    using Application.Claim.Commands.DeleteClaim;
    using Application.Claim.Queries.GetClaimList;
    using Application.Infrastructure.AspNet.Extensions;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Authorize(Roles = "auth.admin")]
    public class ClaimController : Controller
    {
        private readonly IMediator _mediator;

        public ClaimController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var claims = await _mediator.Send(new GetClaimListQuery());
            
            return View(claims);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClaimCommand command)
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
        public async Task<IActionResult> Delete(string type)
        {
            await _mediator.Send(new DeleteClaimCommand { Type = type });

            return RedirectToAction("Index");
        }
    }
}