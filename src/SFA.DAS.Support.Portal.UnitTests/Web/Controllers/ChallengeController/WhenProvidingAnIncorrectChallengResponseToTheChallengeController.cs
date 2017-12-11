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
using SFA.DAS.Support.Portal.Web.Services;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.ChallengeController
{
    [TestFixture]
    public class
        WhenProvidingAnIncorrectChallengResponseToTheChallengeController : WhenTestingAnMvcControllerOfType<
            Portal.Web.Controllers.ChallengeController>
    {
        private Mock<IAccountRepository> _accountRepository;
        private Mock<IGrantPermissions> _granter;
        private Mock<IMediator> _mediator;
        private Mock<HttpContextBase> _mockContextBase;
        private ControllerContext _unitControllerContext;

        [SetUp]
        public override void Setup()
        {
            _granter = new Mock<IGrantPermissions>();
            _accountRepository = new Mock<IAccountRepository>();
            _mediator = new Mock<IMediator>();

            Unit = new Portal.Web.Controllers.ChallengeController(
                _granter.Object,
                _accountRepository.Object,
                _mediator.Object);


            _mockContextBase = new Mock<HttpContextBase>();

            _mockContextBase.Setup(x => x.Request).Returns(new Mock<HttpRequestBase>().Object);
            _mockContextBase.Setup(x => x.Response).Returns(new Mock<HttpResponseBase>().Object);
            _mockContextBase.Setup(x => x.User).Returns(new Mock<IPrincipal>().Object);

            _unitControllerContext = new ControllerContext(_mockContextBase.Object, new RouteData(), Unit);

            Unit.ControllerContext = _unitControllerContext;
        }
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

            _mediator.Setup(x => x.SendAsync(It.IsAny<ChallengePermissionQuery>()))
                .Returns(Task.FromResult(challengePermissionResponse));
            _mockContextBase.Setup(x => x.Request.CurrentExecutionFilePath).Returns("SomePathOrOther");
            // needs to mock 'Request.CurrentExecutionFilePath'
            ActionResultResponse = Unit.Index(challengeEntry).Result;

            Assert.IsInstanceOf<RedirectResult>(ActionResultResponse);
            var url = ((RedirectResult) ActionResultResponse).Url;

            Assert.AreEqual($"SomePathOrOther?url={challengeEntry.Url}&hasError=true", url);
        }
    }
}