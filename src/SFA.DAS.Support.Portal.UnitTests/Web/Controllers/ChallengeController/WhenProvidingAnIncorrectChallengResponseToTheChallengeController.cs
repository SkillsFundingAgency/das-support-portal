using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Core.Domain.Model;
using SFA.DAS.Support.Portal.Web.Models;
using SFA.DAS.Support.Portal.Web.Services;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.ChallengeController
{
    public abstract class
        WhenTestingTheChallengeController : WhenTestingAnMvcControllerOfType<Portal.Web.Controllers.ChallengeController>
    {
        protected Mock<IAccountRepository> AccountRepository;
        protected Mock<IGrantPermissions> MockGranter;
        protected Mock<IMediator> MockMediator;
        protected Mock<HttpContextBase> MockContextBase;
        protected ControllerContext UnitControllerContext;
        protected Mock<HttpResponseBase> MockResponse;
        protected Mock<IPrincipal> MockUser;

        [SetUp]
        public override void Setup()
        {
            MockGranter = new Mock<IGrantPermissions>();
            AccountRepository = new Mock<IAccountRepository>();
            MockMediator = new Mock<IMediator>();

            Unit = new Portal.Web.Controllers.ChallengeController(
                MockGranter.Object,
                AccountRepository.Object,
                MockMediator.Object);


            MockContextBase = new Mock<HttpContextBase>();

            MockContextBase.Setup(x => x.Request).Returns(new Mock<HttpRequestBase>().Object);
            MockResponse = new Mock<HttpResponseBase>();
            MockContextBase.Setup(x => x.Response).Returns(MockResponse.Object);
            MockUser = new Mock<IPrincipal>();
            MockContextBase.Setup(x => x.User).Returns(MockUser.Object);

            UnitControllerContext = new ControllerContext(MockContextBase.Object, new RouteData(), Unit);

            Unit.ControllerContext = UnitControllerContext;
        }
    }
    [TestFixture]
    public class WhenReIssuingAChallenge : WhenTestingTheChallengeController
    {
        [Test]
        public async Task ItShouldReturnTheChallengeView()
        {
            var id = "123";
            var url = "https://tempuri.org/challenge";

            MockMediator.Setup(x => x.SendAsync(It.IsAny<ChallengeQuery>()))
                .ReturnsAsync(new ChallengeResponse()
                {
                    Account = new Account(),
                    StatusCode = SearchResponseCodes.Success,
                    Characters = new List<int>() { 1, 2 }
                });
            var actual = await Unit.Index(id, url, true);

            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<ViewResult>(actual);

            Assert.IsInstanceOf<ChallengeViewModel>(((ViewResult)actual).Model);
        }
    }

    [TestFixture]
    public class WhenFirstIssuingAChallenge : WhenTestingTheChallengeController
    {
        [Test]
        public async Task ItShouldReturnTheChallengeView()
        {
            var id = "123";
            var url = "https://tempuri.org/challenge";

            MockMediator.Setup(x => x.SendAsync(It.IsAny<ChallengeQuery>()))
                .ReturnsAsync(new ChallengeResponse()
                {
                    Account = new Account(),
                    StatusCode = SearchResponseCodes.Success,
                    Characters = new List<int>() { 1, 2 }
                });
            var actual = await Unit.Index(id, url, false);

            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<ViewResult>(actual);

            Assert.IsInstanceOf<ChallengeViewModel>(((ViewResult)actual).Model);
        }
    }

    [TestFixture]
    public class
        WhenProvidingACorrectChallengResponseToTheChallengeController : WhenTestingTheChallengeController
    {
        [Test]
        public void ItShouldRedirectWithoutAnError()
        {
            var challengeEntry = new ChallengeEntry
            {
                Balance = "100",
                Challenge1 = "Challenge1",
                Challenge2 = "Challenge2",
                FirstCharacterPosition = "1",
                SecondCharacterPosition = "2",
                Id = "123123",
                Url = "some/url"
            };

            var challengePermissionResponse = new ChallengePermissionResponse
            {
                Id = "123123",
                IsValid = true,
                Url = "some/url"
            };

            MockMediator.Setup(x => x.SendAsync(It.IsAny<ChallengePermissionQuery>()))
                .Returns(Task.FromResult(challengePermissionResponse));

            MockContextBase.SetupGet(x => x.Request.CurrentExecutionFilePath).Returns("SomePathOrOther");

            
            ActionResultResponse = Unit.Index(challengeEntry).Result;

            MockGranter.Verify(x => x.GivePermissions(MockResponse.Object, MockUser.Object, challengeEntry.Id)
            , Times.Once);
            
            Assert.IsInstanceOf<RedirectResult>(ActionResultResponse);

            var url = ((RedirectResult)ActionResultResponse).Url;

            Assert.AreEqual(challengeEntry.Url, url);
            Assert.IsFalse(url.Contains("hasError=true"));

        }
    }
    [TestFixture]
    public class
        WhenProvidingAnIncorrectChallengResponseToTheChallengeController : WhenTestingTheChallengeController
    {
        [Test]
        public void ItShouldRedirectWithAnError()
        {
            var challengeEntry = new ChallengeEntry
            {
                Balance = "100",
                Challenge1 = "Challenge1",
                Challenge2 = "Challenge2",
                FirstCharacterPosition = "1",
                SecondCharacterPosition = "2",
                Id = "123123",
                Url = "some/url"
            };

            var challengePermissionResponse = new ChallengePermissionResponse
            {
                Id = "123123",
                IsValid = false,
                Url = "some/url"
            };

            MockMediator.Setup(x => x.SendAsync(It.IsAny<ChallengePermissionQuery>()))
                .Returns(Task.FromResult(challengePermissionResponse));
            MockContextBase.Setup(x => x.Request.CurrentExecutionFilePath).Returns("SomePathOrOther");
            
            ActionResultResponse = Unit.Index(challengeEntry).Result;

            Assert.IsInstanceOf<RedirectResult>(ActionResultResponse);
            var url = ((RedirectResult)ActionResultResponse).Url;

            Assert.AreEqual($"SomePathOrOther?url={challengeEntry.Url}&hasError=true", url);
        }
    }
}