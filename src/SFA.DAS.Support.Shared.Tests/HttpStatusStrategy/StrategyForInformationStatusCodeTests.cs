using System.Net;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Shared.Tests.HttpStatusStrategy
{
    [TestFixture]
    public class StrategyForInformationStatusCodeTests : StrategyTestBase<StrategyForInformationStatusCode>
    {
        [TestCase(HttpStatusCode.Continue)]
        [TestCase(HttpStatusCode.SwitchingProtocols)]
        public void ItShouldDecideToReturnNullWithCode(HttpStatusCode code)
        {
            var actual = Unit.Handle(_httpClient, code);
            var expected = HttpStatusCodeDecision.ReturnNull;
            Assert.AreEqual(expected, actual);
        }


        [TestCase(HttpStatusCode.Continue)]
        [TestCase(HttpStatusCode.SwitchingProtocols)]
        public void ItShouldLogInformationWithCode(HttpStatusCode code)
        {
            var actual = Unit.Handle(_httpClient, code);
            MockLogger.Verify(x => x.Info(It.IsAny<string>()), Times.Once());
        }
    }
}