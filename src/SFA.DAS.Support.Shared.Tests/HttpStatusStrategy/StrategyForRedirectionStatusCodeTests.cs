using System.Net;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Shared.Tests.HttpStatusStrategy
{
    [TestFixture]
    public class StrategyForRedirectionStatusCodeTests : StrategyTestBase<StrategyForRedirectionStatusCode>
    {
        [TestCase(HttpStatusCode.MultipleChoices)]
        [TestCase(HttpStatusCode.Ambiguous)]
        [TestCase(HttpStatusCode.MovedPermanently)]
        [TestCase(HttpStatusCode.Moved)]
        [TestCase(HttpStatusCode.Found)]
        [TestCase(HttpStatusCode.Redirect)]
        [TestCase(HttpStatusCode.SeeOther)]
        [TestCase(HttpStatusCode.RedirectMethod)]
        [TestCase(HttpStatusCode.NotModified)]
        [TestCase(HttpStatusCode.UseProxy)]
        [TestCase(HttpStatusCode.Unused)]
        [TestCase(HttpStatusCode.TemporaryRedirect)]
        [TestCase(HttpStatusCode.RedirectKeepVerb)]
        public void ItShouldLogInfoAndDecideToReturnNullWithCode(HttpStatusCode code)
        {
            var actual = Unit.Handle(_httpClient, code);
            var expected = HttpStatusCodeDecision.ReturnNull;
            Assert.AreEqual(expected, actual);
            MockLogger.Verify(x => x.Info(It.IsAny<string>()), Times.Once());
        }
    }
}