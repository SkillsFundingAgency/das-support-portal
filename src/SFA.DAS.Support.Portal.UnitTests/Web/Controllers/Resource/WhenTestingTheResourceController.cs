using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Models;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.Web.Controllers;
using SFA.DAS.Support.Portal.Web.Services;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.Resource
{
    [TestFixture]
    public class WhenTestingTheResourceController : 
        WhenTestingAnMvcControllerOfType<ResourceController>
    {

        private Mock<IManifestRepository> _repository;
        private Mock<ICheckPermissions> _checker;
        private Mock<IGrantPermissions> _granter;

        [SetUp]
        public override void Setup()
        {
            _repository = new Mock<IManifestRepository>();
            _checker = new Mock<ICheckPermissions>();
            _granter = new Mock<IGrantPermissions>();
            Unit = new ResourceController(_repository.Object, _checker.Object, _granter.Object);
        }
        /// <summary>
        /// TODO Test Get Index , Post Challenge
        /// </summary>
        [Test]
        public void ItShouldTestTheUnitBehaviour()
        {
            
            Assert.Fail("To be implemented");
        }

        [Test]
        public async Task ItShouldProvideTheChallengeSubView()
        {
            string id = "id";
            string resourceId = "resourceId";
            string key = "key";
            string url = "";
            _repository.Setup(x => x.ChallengeExists(key)).Returns(Task.FromResult(true));
            NavViewModel navResponse = new NavViewModel() { Current = "", Items = new NavItem[] { new NavItem() { Href = "", Key = "", Title = "" }, } };

            _repository.Setup(x => x.GetNav(key, resourceId)).Returns(Task.FromResult(navResponse));
            _repository.Setup(x => x.GenerateHeader(key, resourceId)).Returns(Task.FromResult(new object() ));
            _repository.Setup(x => x.GetChallengeForm(key, resourceId, url)).Returns(Task.FromResult("<div></div."));

            ActionResultResponse =  await Unit.Challenge(id, resourceId, key , url);

            Assert.IsInstanceOf<ViewResult>(ActionResultResponse);
            var viewResult = ((ViewResult)ActionResultResponse);

            Assert.IsInstanceOf<NavViewModel>(viewResult.ViewBag.SubNav);
            Assert.IsInstanceOf<object>(viewResult.ViewBag.SubHeader);
            Assert.AreEqual("Sub", viewResult.ViewName);
            Assert.IsInstanceOf<string>(viewResult.Model);

        }


        [Test]
        public async Task ItShouldProvideTheChallengeMissingViewIfGetFormThrowsException()
        {
            string id = "id";
            string resourceId = "resourceId";
            string key = "key";
            string url = "";
            _repository.Setup(x => x.ChallengeExists(key)).Returns(Task.FromResult(true));
            NavViewModel navResponse = new NavViewModel() { Current = "", Items = new NavItem[] { new NavItem() { Href = "", Key = "", Title = "" }, } };
            _repository.Setup(x => x.GetNav(key, resourceId)).Returns(Task.FromResult(navResponse));
            _repository.Setup(x => x.GenerateHeader(key, resourceId)).Returns(Task.FromResult(new object()));

            _repository.Setup(x => x.GetChallengeForm(key, resourceId, url)).Throws<Exception>();


            ActionResultResponse = await Unit.Challenge(id, resourceId, key, url);

            Assert.IsInstanceOf<ViewResult>(ActionResultResponse);
            var viewResult = ((ViewResult)ActionResultResponse);

            Assert.IsInstanceOf<NavViewModel>(viewResult.ViewBag.SubNav);
            Assert.IsInstanceOf<object>(viewResult.ViewBag.SubHeader);
            Assert.AreEqual("Missing", viewResult.ViewName);
            Assert.IsNull(viewResult.Model);
        }

    }
}
