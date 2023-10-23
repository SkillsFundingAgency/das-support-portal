using System.Configuration;
using SFA.DAS.Support.Portal.Web.Settings;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using SFA.DAS.Support.Portal.Web.Models.Error;

namespace SFA.DAS.Support.Portal.Web.Controllers
{
    [ExcludeFromCodeCoverage]
    [AllowAnonymous]
    public sealed class ErrorController : Controller
    {
        private readonly IWebConfiguration _webConfiguration;

        public ErrorController(IWebConfiguration webConfiguration)
        {
            _webConfiguration = webConfiguration;
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult BadRequest()
        {
            Response.StatusCode = 400;

            return View("Error400");
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult NotFound()
        {
            Response.StatusCode = 404;

            return View("Error404");
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult Error()
        {
            Response.StatusCode = 500;

            return View("Error500");
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult NoError()
        {
            return View("Error404");
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult Forbidden()
        {
            var resourceEnvironmentName = "TEST";//ConfigurationManager.AppSettings["ResourceEnvironmentName"];

            //Response.StatusCode = 403;

            return View("Error403", new Error403ViewModel(resourceEnvironmentName){ UseDfESignIn = _webConfiguration.UseDfESignIn });
        }
    }
}