using System.Web.Mvc;

namespace SFA.DAS.Portal.Web.Controllers
{
    [AllowAnonymous]
    public sealed class ErrorController : Controller
    {

        // GET: Error
        [AllowAnonymous]
        public ViewResult BadRequest()
        {
            Response.StatusCode = 400;

            return View("Error400");
        }

        [AllowAnonymous]
        public ViewResult NotFound()
        {
            Response.StatusCode = 404;

            return View("Error404");
        }

        [AllowAnonymous]
        public ViewResult Error()
        {
            Response.StatusCode = 500;

            return View("Error500");
        }

        [AllowAnonymous]
        public ViewResult NoError()
        {
            return View("Error404");
        }
    }
}