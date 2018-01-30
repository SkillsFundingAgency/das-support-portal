using System;
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
        public async Task<ActionResult> Index(SearchQuery query)
        {
            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                query.SearchTerm = query.SearchTerm.Trim();

                var response = await _mediator.SendAsync(query);

                var viewModel = _mappingService.Map<SearchResponse, SearchResultsViewModel>(response);
                return View(viewModel);
            }

            return View(new SearchResultsViewModel());
        }

        public ActionResult Detail(string id)
        {
            return View();
        }
    }
}