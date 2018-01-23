using System;
using System.Net;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Shared.Tests.HttpStatusStrategy
{
    [TestFixture]
    public class StrategyForClientErrorStatusCodeTests : StrategyTestBase<StrategyForClientErrorStatusCode>
    {
        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.PaymentRequired)]
        [TestCase(HttpStatusCode.MethodNotAllowed)]
        [TestCase(HttpStatusCode.NotAcceptable)]
        [TestCase(HttpStatusCode.ProxyAuthenticationRequired)]
        [TestCase(HttpStatusCode.RequestTimeout)]
        [TestCase(HttpStatusCode.Conflict)]
        [TestCase(HttpStatusCode.Gone)]
        [TestCase(HttpStatusCode.LengthRequired)]
        [TestCase(HttpStatusCode.PreconditionFailed)]
        [TestCase(HttpStatusCode.RequestEntityTooLarge)]
        [TestCase(HttpStatusCode.RequestUriTooLong)]
        [TestCase(HttpStatusCode.UnsupportedMediaType)]
        [TestCase(HttpStatusCode.RequestedRangeNotSatisfiable)]
        [TestCase(HttpStatusCode.ExpectationFailed)]
        [TestCase(HttpStatusCode.UpgradeRequired)]
        public void ItShouldLogErrorThenDecideToReturnNullWithCode(HttpStatusCode code)
        {
            var actual = Unit.Handle(_httpClient, code);
            var expected = HttpStatusCodeDecision.ReturnNull;
            Assert.AreEqual(expected, actual);
            MockLogger.Verify(x => x.Error(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once());
            Assert.IsNotNull(_httpClient.DefaultRequestHeaders.Authorization);
        }


        [TestCase(HttpStatusCode.Unauthorized)]
        public void ItShouldLogWarningDecideToReturnNullAndRemoveAuthorisationHeaderWithCode(HttpStatusCode code)
        {
            var actual = Unit.Handle(_httpClient, code);
            var expected = HttpStatusCodeDecision.ReturnNull;
            Assert.AreEqual(expected, actual);
            MockLogger.Verify(x => x.Warn(It.IsAny<string>()), Times.Once());
            Assert.IsNull(_httpClient.DefaultRequestHeaders.Authorization);
        }

        [TestCase(HttpStatusCode.Forbidden)]
        public void ItShouldLogWarningDecideToReturnNullAndNotRemoveAuthorisationHeaderWithCode(HttpStatusCode code)
        {
            var actual = Unit.Handle(_httpClient, code);
            var expected = HttpStatusCodeDecision.ReturnNull;
            Assert.AreEqual(expected, actual);
            MockLogger.Verify(x => x.Warn(It.IsAny<string>()), Times.Once());
            Assert.IsNotNull(_httpClient.DefaultRequestHeaders.Authorization);
        }


        [TestCase(HttpStatusCode.NotFound)]
        public void ItShouldLogWarnAndDecideToReturnNullWithCode(HttpStatusCode code)
        {
            var actual = Unit.Handle(_httpClient, code);
            var expected = HttpStatusCodeDecision.ReturnNull;
            Assert.AreEqual(expected, actual);
            MockLogger.Verify(x => x.Warn(It.IsAny<string>()), Times.Once());
        }
    }
}