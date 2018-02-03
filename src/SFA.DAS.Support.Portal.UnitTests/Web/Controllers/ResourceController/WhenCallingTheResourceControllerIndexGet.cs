using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Models;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.ResourceController
{
    [TestFixture]
    public class WhenCallingTheResourceControllerIndexGet : WhenTestingTheResourceController
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _id = "id";
            _resourceId = "resourceId";
            _key = SupportServiceResourceKey.EmployerUserAccountTeam;
            _url = "";
        }

        private string _id;
        private string _resourceId;
        private SupportServiceResourceKey _key;
        private string _url;


        /// <summary>
        ///     See <see cref="WhenTestingTheResourceController" /> for details of mocking HttpContext
        ///     Where hard coupling to [Http]Request is taken care of.
        /// </summary>
        [Test]
        public async Task ItShouldRedirectToChallengeIfTheResourceDefinesAChallengeThatHasNotAlreadyBeenPassed()
        {
            MockContextBase.Setup(x => x.Request.RawUrl).Returns("https:/tempuri.org");

            MockManifestRepository.Setup(x => x.ResourceExists(_key)).Returns(Task.FromResult(true));
            var siteResource = new SiteResource {Challenge = "Some challenge"};
            MockManifestRepository.Setup(x => x.GetResource(_key)).Returns(Task.FromResult(siteResource));
            MockPermissionsChecker.Setup(x => x.HasPermissions(It.IsAny<HttpRequestBase>(),
                    It.IsAny<HttpResponseBase>(), It.IsAny<IPrincipal>(), $"{_key.ToString()}/{_id}"))
                .Returns(false);

            ActionResultResponse = await Unit.Index(_key, _id);

            Assert.IsInstanceOf<RedirectToRouteResult>(ActionResultResponse);
            var result = (RedirectToRouteResult) ActionResultResponse;

            Assert.IsNotEmpty(result.RouteValues);
            Assert.AreEqual(siteResource.Challenge, result.RouteValues["key"]);
            Assert.AreEqual(_id, result.RouteValues["resourceId"]);
            Assert.AreEqual(MockContextBase.Object.Request.RawUrl, result.RouteValues["url"]);
        }

        [Test]
        public async Task ItShouldReturnABasicWarningViewIfResourceDoesnotExist()
        {
            MockManifestRepository.Setup(x => x.ResourceExists(_key)).Returns(Task.FromResult(false));

            ActionResultResponse = await Unit.Index(_key, _id);

            Assert.IsInstanceOf<ViewResult>(ActionResultResponse);
            var viewResult = (ViewResult) ActionResultResponse;

            Assert.AreEqual("Sub", viewResult.ViewName);
            Assert.IsInstanceOf<ResourceResultModel>(viewResult.Model);
        }

        /// <summary>
        ///     See <see cref="WhenTestingTheResourceController" /> for details of mocking HttpContext
        ///     Where hard coupling to [Http]Request is taken care of.
        /// </summary>
        [Test]
        public async Task ItShouldReturnTheSubviewIfTheResourceDefinesAChallengethatHasAlredyBeenPassed()
        {
            MockContextBase.Setup(x => x.Request.RawUrl).Returns("https:/tempuri.org");

            MockManifestRepository.Setup(x => x.ResourceExists(_key)).Returns(Task.FromResult(true));
            var siteResource = new SiteResource {Challenge = "A challenge"};
            MockManifestRepository.Setup(x => x.GetResource(_key)).Returns(Task.FromResult(siteResource));
            MockPermissionsChecker.Setup(x => x.HasPermissions(It.IsAny<HttpRequestBase>(),
                    It.IsAny<HttpResponseBase>(), It.IsAny<IPrincipal>(), $"{_key}/{_id}"))
                .Returns(true);

            ActionResultResponse = await Unit.Index(_key, _id);

            Assert.IsInstanceOf<ViewResult>(ActionResultResponse);
            var result = (ViewResult) ActionResultResponse;

            Assert.AreEqual("Sub", result.ViewName);
        }

        /// <summary>
        ///     See <see cref="WhenTestingTheResourceController" /> for details of mocking HttpContext
        ///     Where hard coupling to [Http]Request is taken care of.
        /// </summary>
        [Test]
        public async Task ItShouldReturnTheSubviewIfTheResourcedoesNotDefineAChallenge()
        {
            MockContextBase.Setup(x => x.Request.RawUrl).Returns("https:/tempuri.org");

            MockManifestRepository.Setup(x => x.ResourceExists(_key)).Returns(Task.FromResult(true));
            var siteResource = new SiteResource {Challenge = null};
            MockManifestRepository.Setup(x => x.GetResource(_key)).Returns(Task.FromResult(siteResource));
            //MockPermissionsChecker.Setup(x => x.HasPermissions(It.IsAny<HttpRequestBase>(), It.IsAny<HttpResponseBase>(), It.IsAny<IPrincipal>(), $"{_key}/{_id}"))
            //    .Returns(false);

            ActionResultResponse = await Unit.Index(_key, _id);

            Assert.IsInstanceOf<ViewResult>(ActionResultResponse);
            var result = (ViewResult) ActionResultResponse;

            Assert.AreEqual("Sub", result.ViewName);
        }
    }
}