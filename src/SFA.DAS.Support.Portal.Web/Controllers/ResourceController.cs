using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using SFA.DAS.Support.Portal.ApplicationServices.Models;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.Web.Services;
using SFA.DAS.Support.Shared.Discovery;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Portal.Web.Controllers
{
    public class ResourceController : Controller
    {
        private readonly ICheckPermissions _checker;
        private readonly IGrantPermissions _granter;
        private readonly IManifestRepository _repository;
        private readonly IServiceConfiguration _serviceConfiguration;
        public ResourceController(
            IManifestRepository repository,
            ICheckPermissions checker,
            IGrantPermissions granter, IServiceConfiguration serviceConfiguration)
        {
            _repository = repository;
            _checker = checker;
            _granter = granter;
            _serviceConfiguration = serviceConfiguration;
        }

        [HttpGet]
        public async Task<ActionResult> Challenge(SupportServiceResourceKey resourceKey, SupportServiceResourceKey challengeKey, string resourceId, string url)
        {
            if (!_serviceConfiguration.ChallengeExists(challengeKey)) return HttpNotFound();

            ViewBag.SubNav = await _repository.GetNav(resourceKey, resourceId);
            ViewBag.SubHeader = await _repository.GenerateHeader(resourceKey, resourceId);

            try
            {
                var challengeForm = await _repository.GetChallengeForm(resourceKey, challengeKey, resourceId, url);
                return View("Sub", new ResourceResultModel
                {
                    StatusCode = HttpStatusCode.OK,
                    Resource = challengeForm
                });
            }
            catch
            {
                return View("Missing");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Challenge(SupportServiceResourceKey resourceKey, SupportServiceResourceKey challengeKey, string resourceId, FormCollection formData)
        {
            var pairs = formData.AllKeys.ToDictionary(k => k, v => formData[v]);
            var result = await _repository.SubmitChallenge(resourceId, pairs);

            if (result.HasRedirect)
            {
                _granter.GivePermissions(Response, User, $"{challengeKey}/{resourceId}");
                return Redirect(result.RedirectUrl);
            }

            ViewBag.SubNav = await _repository.GetNav(resourceKey, resourceId);
            ViewBag.SubHeader = await _repository.GenerateHeader(resourceKey, resourceId);
            return View("Sub", new ResourceResultModel
            {
                StatusCode = HttpStatusCode.OK,
                Resource = result.Page
            });
        }

        [HttpGet]
        [Route("resource")]
        public async Task<ActionResult> Index(SupportServiceResourceKey key, string id, string childId)
        {
            id = id.ToUpper();
            if (!_serviceConfiguration.ResourceExists(key))
                return View("Sub",
                    new ResourceResultModel
                    {
                        Resource = "<h3>This resource isn't registered</h3>",
                        StatusCode = HttpStatusCode.OK,
                        Exception = null
                    });

            var resource = _serviceConfiguration.GetResource(key);

            if (resource.Challenge.HasValue)
            {
                if (!_checker.HasPermissions(Request, Response, User, $"{resource.Challenge}/{id}"))
                {
                    return RedirectToAction("Challenge",
                                            new
                                            {
                                                resourceId = id,
                                                resourceKey = (int)key,
                                                challengeKey = (int)resource.Challenge,
                                                url = Request.RawUrl
                                            });
                }
            }

            ViewBag.SubNav = await _repository.GetNav(key, id);
            ViewBag.SubHeader = await _repository.GenerateHeader(key, id);

            var resourceResult = await _repository.GetResourcePage(key, id, childId);

            return View("Sub", resourceResult);
        }

        [HttpGet]
        [Route("resource/apprenticeships/search/{hashedAccountId}")]
        public async Task<ActionResult> Apprenticeships(string hashedAccountId, ApprenticeshipSearchType searchType, string searchTerm)
        {            
            ViewBag.SubNav = await _repository.GetNav(SupportServiceResourceKey.CommitmentSearch, hashedAccountId);
            ViewBag.SubHeader = await _repository.GenerateHeader(SupportServiceResourceKey.CommitmentSearch, hashedAccountId);
           
            var resourceResult = await _repository.SubmitApprenticeSearchRequest(
                                                SupportServiceResourceKey.CommitmentSearch, 
                                                hashedAccountId,
                                                searchType, 
                                                searchTerm);

            return View("sub", resourceResult);
        }
    }
}