using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Models;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Shared.Discovery;
using SFA.DAS.Support.Shared.Navigation;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.ResourceController
{
    [TestFixture]
    public class WhenCallingTheResourceControllerChallengePost : WhenTestingTheResourceController
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _id = "id";
            _resourceId = "resourceId";
            _challengeKey = SupportServiceResourceKey.EmployerAccountFinance;
            _resourceKey = SupportServiceResourceKey.EmployerAccountFinanceChallenge;
            _url = "";
            _formCollection = new FormCollection();
            _formCollection.Add("TestKey", "TestValue");
        }

        private string _id;
        private string _resourceId;
        private SupportServiceResourceKey _challengeKey;
        private SupportServiceResourceKey _resourceKey;
        private string _url;
        private FormCollection _formCollection;

        [Test]
        public async Task ItShouldRedirectIfTheSubmittedChallengeHasARedirect()
        {
            var challengeResult = new ChallengeResult { Page = "1", RedirectUrl = "http://tempuri.org" };

            MockManifestRepository
                .Setup(x => x.SubmitChallenge(_resourceId, It.IsAny<Dictionary<string, string>>()))
                .Returns(Task.FromResult(challengeResult));

            ActionResultResponse = await Unit.Challenge(_resourceKey, _challengeKey, _resourceId, _formCollection);

            MockPermissionsGranter
                .Verify(
                    x => x.GivePermissions(It.IsAny<HttpResponseBase>(), It.IsAny<IPrincipal>(), It.IsAny<string>()),
                    Times.Once);

            Assert.IsInstanceOf<RedirectResult>(ActionResultResponse);
        }

        [Test]
        public async Task ItShouldReturnAnUnRedirectedSubViewofTheChallengePage()
        {
            var challengeResult = new ChallengeResult { Page = "<html><div>Challenge</div></<html>", RedirectUrl = null };
            var navResponse =
                new NavViewModel
                {
                    Current = SupportServiceResourceKey.EmployerUserAccountTeam,
                    Items = new[]
                        {new NavItem {Href = "", Key = SupportServiceResourceKey.EmployerUserAccountTeam, Title = ""}}
                };

            MockManifestRepository.Setup(x => x.SubmitChallenge(_resourceId, It.IsAny<Dictionary<string, string>>()))
                .Returns(Task.FromResult(challengeResult));


            MockManifestRepository.Setup(x => x.GetNav(_resourceKey, _resourceId)).Returns(Task.FromResult(navResponse));
            MockManifestRepository.Setup(x => x.GenerateHeader(_resourceKey, _resourceId))
                .Returns(Task.FromResult(new ResourceResultModel()));


            ActionResultResponse = await Unit.Challenge(_resourceKey, _challengeKey, _resourceId, _formCollection);


            Assert.IsInstanceOf<ViewResult>(ActionResultResponse);
            var viewResult = (ViewResult)ActionResultResponse;

            Assert.IsInstanceOf<NavViewModel>(viewResult.ViewBag.SubNav);
            Assert.IsInstanceOf<object>(viewResult.ViewBag.SubHeader);
            Assert.AreEqual("Sub", viewResult.ViewName);
            Assert.IsInstanceOf<ResourceResultModel>(viewResult.Model);
            Assert.AreEqual(challengeResult.Page, ((ResourceResultModel)viewResult.Model).Resource);
        }
    }
}