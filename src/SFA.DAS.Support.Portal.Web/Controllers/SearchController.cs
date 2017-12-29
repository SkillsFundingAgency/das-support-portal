using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Web.Services;
using SFA.DAS.Support.Portal.Web.ViewModels;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Shared;
using System.Collections.Generic;

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
            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                query.SearchTerm = query.SearchTerm.Trim();

                if (Request.Headers.AllKeys.Contains("new"))
                {
                    var response = await _mediator.SendAsync(new SearchQuery { Query = query.SearchTerm, Page = query.Page , SearchType = query.SearchType});
                    var viewModel = _mappingService.Map<SearchResponse, SearchResultsViewModel>(response);
                    return View(viewModel);
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