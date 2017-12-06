using System.Web.Mvc;

namespace SFA.DAS.Support.Portal.Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

    }
}