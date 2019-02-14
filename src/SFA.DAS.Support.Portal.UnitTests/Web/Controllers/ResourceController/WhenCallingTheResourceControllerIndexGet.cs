using System.Linq;
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

        private string _id;
        private SupportServiceResourceKey _resourceKey;
        private string _childId;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _id = "ID";
            _resourceKey = SupportServiceResourceKey.EmployerAccountFinance;
            _childId = "childId";
        }

        [Test]
        public async Task ItShouldConvertAccountIdToUpper()
        {
            var idLower = _id.ToLower();
            MockContextBase.Setup(x => x.Request.RawUrl).Returns("https:/tempuri.org");

            var siteResource = new SiteResource { ResourceKey = SupportServiceResourceKey.EmployerAccountFinance, Challenge = SupportServiceResourceKey.EmployerAccountFinanceChallenge };
            MockPermissionsChecker.Setup(x => x.HasPermissions(It.IsAny<HttpRequestBase>(),
                    It.IsAny<HttpResponseBase>(),
                    It.IsAny<IPrincipal>(),
                    $"{_resourceKey.ToString()}/{_id}"))
                .Returns(false);

            ActionResultResponse = await Unit.Index(_resourceKey, idLower, _childId);

            MockManifestRepository.Setup(x => x.GetNav(It.Is<SupportServiceResourceKey>(rk => rk == _resourceKey), It.Is<string>(id => id == _id.ToUpper())));

            Assert.IsInstanceOf<RedirectToRouteResult>(ActionResultResponse);
            var result = (RedirectToRouteResult)ActionResultResponse;

            Assert.AreEqual(result.RouteValues["resourceid"], _id);
        }

        /// <summary>
        ///     See <see cref="WhenTestingTheResourceController" /> for details of mocking HttpContext
        ///     Where hard coupling to [Http]Request is taken care of.
        /// </summary>
        [Test]
        public async Task ItShouldRedirectToChallengeIfTheResourceDefinesAChallengeThatHasNotAlreadyBeenPassed()
        {
            MockContextBase.Setup(x => x.Request.RawUrl).Returns("https:/tempuri.org");

            var siteResource = new SiteResource { ResourceKey = SupportServiceResourceKey.EmployerAccountFinance, Challenge = SupportServiceResourceKey.EmployerAccountFinanceChallenge };
            MockPermissionsChecker.Setup(x => x.HasPermissions(It.IsAny<HttpRequestBase>(),
                    It.IsAny<HttpResponseBase>(),
                    It.IsAny<IPrincipal>(),
                    $"{_resourceKey.ToString()}/{_id}"))
                .Returns(false);

            ActionResultResponse = await Unit.Index(_resourceKey, _id, _childId);

            Assert.IsInstanceOf<RedirectToRouteResult>(ActionResultResponse);
            var result = (RedirectToRouteResult)ActionResultResponse;

            Assert.IsNotEmpty(result.RouteValues);
            Assert.AreEqual((int)siteResource.ResourceKey, result.RouteValues["resourceKey"]);
            Assert.AreEqual((int)siteResource.Challenge, result.RouteValues["challengeKey"]);
            Assert.AreEqual(_id, result.RouteValues["resourceId"]);
            Assert.AreEqual(MockContextBase.Object.Request.RawUrl, result.RouteValues["url"]);
        }

        [Test]
        public async Task ItShouldReturnABasicWarningViewIfResourceDoesnotExist()
        {
            _resourceKey = SupportServiceResourceKey.None;

            ActionResultResponse = await Unit.Index(_resourceKey, _id, _childId);

            Assert.IsInstanceOf<ViewResult>(ActionResultResponse);
            var viewResult = (ViewResult)ActionResultResponse;

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

            var siteResource = new SiteResource
            {
                ResourceKey = SupportServiceResourceKey.EmployerAccountFinance,
                Challenge = SupportServiceResourceKey.EmployerAccountFinanceChallenge
            };

            MockPermissionsChecker
                .Setup(x => x.HasPermissions(It.IsAny<HttpRequestBase>(),
                    It.IsAny<HttpResponseBase>(),
                    It.IsAny<IPrincipal>(),
                    $"{SupportServiceResourceKey.EmployerAccountFinanceChallenge}/{_id}"))
                .Returns(true);

            ActionResultResponse = await Unit.Index(_resourceKey, _id, _childId);

            Assert.IsInstanceOf<ViewResult>(ActionResultResponse);
            var result = (ViewResult)ActionResultResponse;

            Assert.AreEqual("Sub", result.ViewName);
        }

        /// <summary>
        ///     See <see cref="WhenTestingTheResourceController" /> for details of mocking HttpContext
        ///     Where hard coupling to [Http]Request is taken care of.
        /// </summary>
        [Test]
        public async Task ItShouldReturnTheSubviewIfTheResourcedoesNotDefineAChallenge()
        {

            _resourceKey = SupportServiceResourceKey.None;

            MockContextBase.Setup(x => x.Request.RawUrl).Returns("https:/tempuri.org");

            ActionResultResponse = await Unit.Index(_resourceKey, _id, _childId);

            Assert.IsInstanceOf<ViewResult>(ActionResultResponse);

            var result = (ViewResult)ActionResultResponse;

            Assert.AreEqual("Sub", result.ViewName);
        }
    }
}