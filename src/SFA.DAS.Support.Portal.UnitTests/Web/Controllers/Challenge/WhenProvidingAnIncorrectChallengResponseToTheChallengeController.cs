using System.Threading.Tasks;
using System.Web.Mvc;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Core.Domain.Model;
using SFA.DAS.Support.Portal.Web.Controllers;
using SFA.DAS.Support.Portal.Web.Services;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.Challenge
{
    [TestFixture]
    public class
        WhenProvidingAnIncorrectChallengResponseToTheChallengeController : WhenTestingAnMvcControllerOfType<
            ChallengeController>
    {
        private Mock<IAccountRepository> _accountRepository;
        private Mock<IGrantPermissions> _granter;
        private Mock<IMediator> _mediator;

        [SetUp]
        public override void Setup()
        {
            _granter = new Mock<IGrantPermissions>();
            _accountRepository = new Mock<IAccountRepository>();
            _mediator = new Mock<IMediator>();

            Unit = new ChallengeController(
                _granter.Object,
                _accountRepository.Object,
                _mediator.Object);
        }
        [Ignore("The Unit has been designed to be untestable, as it directly references the Request")]
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


            ActionResultResponse = Unit.Index(challengeEntry).Result;

            Assert.IsInstanceOf<RedirectResult>(ActionResultResponse);
            var url = ((RedirectResult) ActionResultResponse).Url;

            Assert.AreEqual($"?url={challengeEntry.Url}&hasError=true", url);
        }
    }
}