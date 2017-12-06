using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using HMRC.ESFA.Levy.Api.Types;
using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Web.Services;
using SFA.DAS.Support.Portal.Web.ViewModels;

namespace SFA.DAS.Support.Portal.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ICheckPermissions _checker;

        public AccountController(
            IMediator mediator,
            ICheckPermissions checker)
        {
            _mediator = mediator;
            _checker = checker;
        }

        public async Task<ActionResult> Index(string id, string searchTerm)
        {
            var response = await _mediator.SendAsync(new AccountDetailOrganisationsQuery(id));

            if (response.StatusCode != SearchResponseCodes.Success) return new HttpNotFoundResult();

            var vm = new AccountDetailViewModel
            {
                Account = response.Account,
                SearchUrl = Url.Action("Index", "Search", new { SearchTerm = searchTerm })
            };

            return View(vm);
        }

        public async Task<ActionResult> TeamMembers(string id, string searchTerm)
        {
            var response = await _mediator.SendAsync(new AccountTeamMembersQuery(id));

            if (response.StatusCode == SearchResponseCodes.Success)
            {
                var vm = new AccountDetailViewModel
                {
                    Account = response.Account,
                    SearchUrl = Url.Action("Index", "Search", new { SearchTerm = searchTerm })
                };

                return View(vm);
            }

            return new HttpNotFoundResult();
        }

        public async Task<ActionResult> PayeSchemes(string id, string searchTerm)
        {
            var response = await _mediator.SendAsync(new AccountPayeSchemesQuery(id));

            if (response.StatusCode == SearchResponseCodes.Success)
            {
                var vm = new AccountDetailViewModel
                {
                    Account = response.Account,
                    SearchUrl = Url.Action("Index", "Search", new { SearchTerm = searchTerm })
                };

                return View(vm);
            }

            return new HttpNotFoundResult();
        }

        public async Task<ActionResult> Finance(string id, string searchTerm)
        {
            if (!_checker.HasPermissions(Request, Response, User, id))
            {
                return RedirectToAction("", "Challenge", new { url = Request.Url?.PathAndQuery, id });
            }

            var response = await _mediator.SendAsync(new AccountFinanceQuery(id));

            if (response.StatusCode == SearchResponseCodes.Success)
            {
                var vm = new FinanceViewModel
                {
                    Account = response.Account,
                    Balance = response.Balance,
                    SearchUrl = Url.Action("Index", "Search", new { SearchTerm = searchTerm })
                };

                return View(vm);
            }

            return new HttpNotFoundResult();
        }

        public async Task<ActionResult> PayeSchemeLevySubmissions(string accountId, string payePosition)
        {
            if (!_checker.HasPermissions(Request, Response, User, accountId))
            {
                return RedirectToAction("", "Challenge", new { url = Request.Url?.PathAndQuery, accountId });
            }

            var response = await _mediator.SendAsync(new AccountLevySubmissionsQuery(accountId, payePosition));

            return View(new PayeSchemeLevySubmissionViewModel
            {
                Account = response.Account,
                LevyDeclarations = response.LevySubmissions?.Declarations ?? new List<Declaration>(),
                PayePosition = payePosition,
                Status = response.StatusCode
            });
        }
    }
}
