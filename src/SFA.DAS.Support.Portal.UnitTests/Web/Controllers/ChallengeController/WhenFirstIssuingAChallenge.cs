using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Core.Domain.Model;
using SFA.DAS.Support.Portal.Web.Models;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.ChallengeController
{
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
}