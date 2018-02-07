using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Models;
using SFA.DAS.Support.Shared.Discovery;
using SFA.DAS.Support.Shared.Navigation;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.ResourceController
{
    [TestFixture]
    public class WhenCallingTheResourceControllerChallengeGet : WhenTestingTheResourceController
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _resourceId = "resourceId";
            _challengeKey = SupportServiceResourceKey.EmployerAccountFinanceChallenge;
            _resourceKey = SupportServiceResourceKey.EmployerAccountFinance;
            _url = "";
        }

        private string _resourceId;
        private SupportServiceResourceKey _challengeKey;
        private SupportServiceResourceKey _resourceKey;
        private string _url;


        [Test]
        public async Task ItShouldProvideTheChallengeMissingViewIfGetChallengeFormThrowsAnyException()
        {


            var navResponse =
                new NavViewModel
                {
                    Current = SupportServiceResourceKey.EmployerAccountFinance,
                    Items = new[]
                        {new NavItem {Href = "", Key = SupportServiceResourceKey.EmployerAccountTeam, Title = ""}}
                };
            MockManifestRepository.Setup(x => x.GetNav(_resourceKey, _resourceId)).Returns(Task.FromResult(navResponse));
            MockManifestRepository.Setup(x => x.GenerateHeader(_resourceKey, _resourceId))
                .Returns(Task.FromResult(new ResourceResultModel()));
            MockManifestRepository.Setup(x => x.GetChallengeForm(_resourceKey, _challengeKey, _resourceId, _url)).Throws<Exception>();


            ActionResultResponse = await Unit.Challenge(_resourceKey, _challengeKey, _resourceId, _url);

            Assert.IsInstanceOf<ViewResult>(ActionResultResponse);
            var viewResult = (ViewResult)ActionResultResponse;

            Assert.IsInstanceOf<NavViewModel>(viewResult.ViewBag.SubNav);
            Assert.IsInstanceOf<object>(viewResult.ViewBag.SubHeader);
            Assert.AreEqual("Missing", viewResult.ViewName);
            Assert.IsNull(viewResult.Model);
        }

        [Test]
        public async Task ItShouldProvideTheChallengeSubView()
        {
            var navResponse =
                new NavViewModel
                {
                    Current = SupportServiceResourceKey.EmployerAccountTeam,
                    Items = new[]
                        {new NavItem {Href = "", Key = SupportServiceResourceKey.EmployerAccountTeam, Title = ""}}
                };
            //MockManifestRepository.Setup(x => x.ChallengeExists(_challengeKey)).Returns(Task.FromResult(true));
            MockManifestRepository.Setup(x => x.GetNav(_resourceKey, _resourceId)).Returns(Task.FromResult(navResponse));
            MockManifestRepository.Setup(x => x.GenerateHeader(_resourceKey, _resourceId))
                .Returns(Task.FromResult(new ResourceResultModel()));
            MockManifestRepository.Setup(x => x.GetChallengeForm(_resourceKey, _challengeKey, _resourceId, _url))
                .Returns(Task.FromResult("<div></div."));

            ActionResultResponse = await Unit.Challenge(_resourceKey, _challengeKey, _resourceId, _url);

            Assert.IsInstanceOf<ViewResult>(ActionResultResponse);
            var viewResult = (ViewResult)ActionResultResponse;

            Assert.IsInstanceOf<NavViewModel>(viewResult.ViewBag.SubNav);
            Assert.IsInstanceOf<object>(viewResult.ViewBag.SubHeader);
            Assert.AreEqual("Sub", viewResult.ViewName);
            Assert.IsInstanceOf<ResourceResultModel>(viewResult.Model);
        }

        [Test]
        public async Task ItShouldReturnHttpNotFoundIfTheChallengeDoesNotExist()
        {
            
            _resourceKey = SupportServiceResourceKey.EmployerAccountFinance;
            _challengeKey = SupportServiceResourceKey.EmployerAccountFinance;

            ActionResultResponse = await Unit.Challenge(_resourceKey, _challengeKey, _resourceId, _url);

            Assert.IsInstanceOf<HttpNotFoundResult>(ActionResultResponse);
        }
    }
}