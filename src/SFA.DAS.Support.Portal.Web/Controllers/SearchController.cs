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
    public class SearchController : Controller
    {
        private readonly IMappingService _mappingService;
        private readonly IMediator _mediator;

        public SearchController(
            IMappingService mappingService,
            IMediator mediator)
        {
            _mappingService = mappingService;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(EmployerUserSearchQuery query)
        {
            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                query.SearchTerm = query.SearchTerm.Trim();

                if (Request.Headers.AllKeys.Contains("new"))
                {
                    var response = await _mediator.SendAsync(new SearchQuery { Query = query.SearchTerm });

                    return View(new SearchResultsViewModel
                    {
                        SearchTerm = query.SearchTerm,
                        NewResults = response.Results
                    });

                }
                else
                {
                    var response = await _mediator.SendAsync(query);

                    if (response.StatusCode != SearchResponseCodes.SearchFailed)
                    {
                        var viewModel = _mappingService.Map<EmployerUserSearchResponse, SearchResultsViewModel>(response);

                        return View(viewModel);
                    }
                }

                return View(new SearchResultsViewModel { ErrorMessage = $"No results found for '{query.SearchTerm}'" });
            }

            return View(new SearchResultsViewModel());
        }

        [HttpGet]
        public async Task<ActionResult> Detail(string id, string searchTerm)
        {
            var response = await _mediator.SendAsync(new EmployerUserQuery(id));

            if (response.StatusCode == SearchResponseCodes.Success)
            {
                var vm = new DetailViewModel
                {
                    User = response.User,
                    SearchUrl = Url.Action("Index", "Search", new { SearchTerm = searchTerm })
                };
                
                return View(vm);
            }

            return new HttpNotFoundResult();
        }
    }
}