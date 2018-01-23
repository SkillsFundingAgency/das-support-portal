using System.Net;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Shared.Tests.HttpStatusStrategy
{
    [TestFixture]
    public class StrategyForSuccessStatusCodeTests : StrategyTestBase<StrategyForSuccessStatusCode>
    {
        [TestCase(HttpStatusCode.OK)]
        [TestCase(HttpStatusCode.Created)]
        [TestCase(HttpStatusCode.Accepted)]
        [TestCase(HttpStatusCode.NonAuthoritativeInformation)]
        [TestCase(HttpStatusCode.NoContent)]
        [TestCase(HttpStatusCode.ResetContent)]
        [TestCase(HttpStatusCode.PartialContent)]
        public void ItShouldLogInfoAndDecideToReturnContinueWithCode(HttpStatusCode code)
        {
            var actual = Unit.Handle(_httpClient, code);
            var expected = HttpStatusCodeDecision.Continue;
            Assert.AreEqual(expected, actual);
            MockLogger.Verify(x => x.Info(It.IsAny<string>()), Times.Once());
        }
    }
}