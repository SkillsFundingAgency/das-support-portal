using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Web.Services;
using SFA.DAS.Support.Portal.Web.ViewModels;

namespace SFA.DAS.Support.Portal.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IMappingService _mappingService;
        private readonly IMediator _mediator;

        public UserController(IMappingService mappingService, IMediator mediator)
        {
            _mappingService = mappingService;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(string id, string searchTerm)
        {
            var response = await _mediator.SendAsync(new EmployerUserQuery(id));

            if (response.StatusCode == SearchResponseCodes.Success)
            {
                var vm = new DetailViewModel
                {
                    User = response.User
                };

                return View(vm);
            }

            return new HttpNotFoundResult();
        }
    }
}