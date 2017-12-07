using System.Collections;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using SFA.DAS.Support.Portal.Health;
using SFA.DAS.Support.Portal.Health.Model;

namespace SFA.DAS.Support.Portal.Web.Controllers
{
    public class HealthController : Controller
    {
        private readonly IHealthService _healthService;

        public HealthController(IHealthService healthService)
        {
            _healthService = healthService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var viewModel = await _healthService.CreateHealthModel();

            if (((IList) Request.AcceptTypes).Contains("application/json"))
                return Content(JsonConvert.SerializeObject(viewModel));

            if (Request.HttpMethod == "HEAD" && viewModel.ApiStatus == Status.Red)
                Response.StatusCode = 500;

            return View(viewModel);
        }

        [Route("Health/Image")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> HealthImage()
        {
            var viewModel = await _healthService.CreateHealthModel();

            var dir = Server.MapPath("/content/dist/img/status");

            var path = Path.Combine(dir, viewModel.ApiStatus == Status.Green ? "green_16.png" : "red_16.png");

            return File(path, "image/png");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> EmployerUser()
        {
            var viewModel = await _healthService.CreateHealthEmployerUserModel();

            if (((IList) Request.AcceptTypes).Contains("application/json"))
                return Content(JsonConvert.SerializeObject(viewModel));

            if (Request.HttpMethod == "HEAD" && viewModel.ApiStatus == Status.Red)
                Response.StatusCode = 500;

            return View(viewModel);
        }

        [Route("Health/EmployerUser/Image")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> EmployerUserImage()
        {
            var viewModel = await _healthService.CreateHealthEmployerUserModel();

            var dir = Server.MapPath("/content/dist/img/status");

            var path = Path.Combine(dir, viewModel.ApiStatus + "_16.png");

            return File(path, "image/png");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Accounts()
        {
            var viewModel = await _healthService.CreateHealthAccountsModel();

            if (((IList) Request.AcceptTypes).Contains("application/json"))
                return Content(JsonConvert.SerializeObject(viewModel));

            if (Request.HttpMethod == "HEAD" && viewModel.ApiStatus == Status.Red)
                Response.StatusCode = 500;

            return View(viewModel);
        }

        [HttpGet]
        [Route("Health/Accounts/Image")]
        [AllowAnonymous]
        public async Task<ActionResult> AccountsImage()
        {
            var viewModel = await _healthService.CreateHealthAccountsModel();

            var dir = Server.MapPath("/content/dist/img/status");

            var path = Path.Combine(dir, viewModel.ApiStatus + "_16.png");

            return File(path, "image/png");
        }
    }
}