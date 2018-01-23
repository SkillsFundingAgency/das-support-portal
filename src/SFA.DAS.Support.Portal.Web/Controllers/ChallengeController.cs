using System.Threading.Tasks;
using System.Web.Mvc;
using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.Core.Domain.Model;
using SFA.DAS.Support.Portal.Web.Models;
using SFA.DAS.Support.Portal.Web.Services;

namespace SFA.DAS.Support.Portal.Web.Controllers
{
    public class ChallengeController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IGrantPermissions _granter;
        private readonly IMediator _mediator;

        public ChallengeController(IGrantPermissions granter,
            IAccountRepository accountRepository,
            IMediator mediator)
        {
            _granter = granter;
            _accountRepository = accountRepository;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(string id, string url, bool hasError = false)
        {
            var response = await _mediator.SendAsync(new ChallengeQuery(id));

            return View(new ChallengeViewModel
            {
                Account = response.Account,
                Characters = response.Characters,
                Id = id,
                Url = url,
                HasError = hasError
            });
        }

        [HttpPost]
        public async Task<ActionResult> Index(ChallengeEntry challengeEntry)
        {
            var response = await _mediator.SendAsync(new ChallengePermissionQuery(challengeEntry));

            var url = challengeEntry.Url.Replace("?hasError=true", string.Empty);

            if (response.IsValid)
            {
                _granter.GivePermissions(Response, User, challengeEntry.Id);
                return Redirect(url);
            }

            return Redirect($"{Request.CurrentExecutionFilePath}?url={challengeEntry.Url}&hasError=true");
        }
    }
}