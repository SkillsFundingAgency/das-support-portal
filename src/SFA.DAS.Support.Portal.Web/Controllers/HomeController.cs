using System.Web.Mvc;
using SFA.DAS.Support.Portal.Web.Settings;
using SFA.DAS.Support.Portal.Web.ViewModels;

namespace SFA.DAS.Support.Portal.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebConfiguration _webConfiguration;

        public HomeController(IWebConfiguration webConfiguration)
        {
            _webConfiguration = webConfiguration;
        }

        [System.Web.Mvc.AllowAnonymous]
        [HttpGet]
        [Route("/")]
        public ActionResult StartPage()
        {
            return View(new StartPageViewModel{ UseDfESignIn = _webConfiguration.UseDfESignIn });
        }
    }
}