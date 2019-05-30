namespace SSO.Server.Controllers
{
    using Application.Error.Queries.GetErrorDetail;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class HomeController : Controller
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Error(string errorId)
        {
            var error = await _mediator.Send(new GetErrorDetailQuery { Id = errorId });

            return View(error);
        }
    }
}