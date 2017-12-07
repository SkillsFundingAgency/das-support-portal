using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Shared;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.ResourceController
{
    [TestFixture]
    public class WhenCallingTheResourceControllerIndexGet: WhenTestingTheResourceController
    {
        private string _id;
        private string _resourceId;
        private string _key;
        private string _url;

       [SetUp]
        public override void Setup()
        {
            base.Setup();
            _id = "id";
            _resourceId = "resourceId";
            _key = "key";
            _url = "";
        }

        [Test]
        public async Task ItShouldReturnABasicWarningViewIfResourceDoesnotExist()
        {
            MockManifestRepository.Setup(x => x.ResourceExists(_key)).Returns(Task.FromResult(false));

            ActionResultResponse = await Unit.Index(_key, _id);

            Assert.IsInstanceOf<ViewResult>(ActionResultResponse);
            var viewResult = ((ViewResult)ActionResultResponse);

            Assert.AreEqual("Sub", viewResult.ViewName);
            Assert.IsInstanceOf<string>(viewResult.Model);

        }

        /// <summary>
        /// See <see cref="WhenTestingTheResourceController"/> for details of mocking HttpContext
        /// Where hard coupling to [Http]Request is taken care of.
        /// </summary>
        [Test]
        public async Task ItShouldRedirectToChallengeIfTheResourceDefinesAChallengeThatHasNotAlreadyBeenPassed()
        {
            MockContextBase.Setup(x => x.Request.RawUrl).Returns("https:/tempuri.org");

            MockManifestRepository.Setup(x => x.ResourceExists(_key)).Returns(Task.FromResult(true));
            var siteResource = new SiteResource(){ Challenge = "Some challenge"};  
            MockManifestRepository.Setup(x => x.GetResource(_key)).Returns(Task.FromResult(siteResource));
            MockPermissionsChecker.Setup(x => x.HasPermissions(It.IsAny<HttpRequestBase>(), It.IsAny<HttpResponseBase>(), It.IsAny<IPrincipal>(), $"{_key}/{_id}"))
                .Returns(false);

            ActionResultResponse = await Unit.Index(_key, _id);

            Assert.IsInstanceOf<RedirectToRouteResult>(ActionResultResponse);
            var result = ((RedirectToRouteResult)ActionResultResponse);

            Assert.IsNotEmpty(result.RouteValues);
            Assert.AreEqual(siteResource.Challenge, result.RouteValues["key"]);
            Assert.AreEqual(_id, result.RouteValues["resourceId"]);
            Assert.AreEqual(MockContextBase.Object.Request.RawUrl, result.RouteValues["url"]);

        }
    }
}