using System.Threading;
using System.Web.Mvc;
using SFA.DAS.Portal.Web.ViewModels;

namespace SFA.DAS.Portal.Web.Controllers
{
    public class SharedController : Controller
    {
        // GET: Shared
        public PartialViewResult Header()
        {
            var viewModel = new HeaderViewModel
            {
                Username = Thread.CurrentPrincipal.Identity.Name
            };

            return PartialView("_Header", viewModel);
        }
    }
}