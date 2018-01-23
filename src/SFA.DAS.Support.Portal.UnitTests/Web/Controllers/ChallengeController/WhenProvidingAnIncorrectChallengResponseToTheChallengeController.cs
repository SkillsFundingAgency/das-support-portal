using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.ChallengeController
{
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
            var url = ((RedirectResult) ActionResultResponse).Url;

            Assert.AreEqual($"SomePathOrOther?url={challengeEntry.Url}&hasError=true", url);
        }
    }
}