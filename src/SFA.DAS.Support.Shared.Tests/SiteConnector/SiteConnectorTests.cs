using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Shared.Tests.SiteConnector
{
    [TestFixture]
    public class SiteConnectorTests : SiteConnectorTestBase
    {
        [TestCase(HttpStatusCode.Ambiguous)] // 300
        [TestCase(HttpStatusCode.BadRequest)] // 400
        [TestCase(HttpStatusCode.Conflict)] // 409
        [TestCase(HttpStatusCode.ExpectationFailed)] // 417
        [TestCase(HttpStatusCode.RequestedRangeNotSatisfiable)] // 416
        [TestCase(HttpStatusCode.BadGateway)] // 502
        public async Task ItShouldReturnNullWhenDownloadTypeRecievesHttpStatus(HttpStatusCode code)
        {
            MockHttpMessageHandler
                .When(TestUrlMatch)
                .Respond(code, "application/json", ValidTestResponseData)
                ;

            var actual = await Unit.Download<TestType>(TestUri);
            Assert.IsNull(Unit.LastException);
            Assert.IsNotNull(Unit.LastContent);

            Assert.AreEqual(code, Unit.LastCode);
            Assert.AreEqual(HttpStatusCodeDecision.ReturnNull, Unit.HttpStatusCodeDecision);
            Assert.IsNotNull(HttpClient.DefaultRequestHeaders.Authorization);
            Assert.IsNull(actual);
        }

        [TestCase(HttpStatusCode.Unauthorized)] // 401
        public async Task ItShouldReturnNullAndStripTheAuthorizationAfterThisCodeIsRecieved(HttpStatusCode code)
        {
            MockHttpMessageHandler
                .When(TestUrlMatch)
                .Respond(code, "application/json", "{}")
                ;

            var actual = await Unit.Download(TestUrlMatch);
            Assert.IsNull(Unit.LastException);
            Assert.IsNotNull(Unit.LastContent);
            Assert.AreEqual(code, Unit.LastCode);
            Assert.AreEqual(HttpStatusCodeDecision.ReturnNull, Unit.HttpStatusCodeDecision);
            Assert.IsNull(HttpClient.DefaultRequestHeaders.Authorization);
            Assert.IsNull(actual);
        }

        [Test]
        public async Task ItShouldDownloadStringSuccessfully()
        {
            MockHttpMessageHandler
                .When(TestUrl)
                .Respond(HttpStatusCode.OK, "application/json", ValidTestResponseData);

            var actual = await Unit.Download(TestUrl);

            Assert.IsNull(Unit.LastException);
            Assert.IsNotNull(Unit.LastContent);
            Assert.AreEqual(HttpStatusCode.OK, Unit.LastCode);
            Assert.AreEqual(HttpStatusCodeDecision.Continue, Unit.HttpStatusCodeDecision);
            Assert.IsNotNull(HttpClient.DefaultRequestHeaders.Authorization);
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<string>(actual);
        }

        [Test]
        public async Task ItShouldDownloadTypeByUriSuccessfully()
        {
            MockHttpMessageHandler
                .When(TestUrl)
                .Respond(HttpStatusCode.OK, "application/json", ValidTestResponseData);

            var actual = await Unit.Download<TestType>(TestUri);


            Assert.IsNull(Unit.LastException);
            Assert.IsNotNull(Unit.LastContent);
            Assert.AreEqual(HttpStatusCode.OK, Unit.LastCode);
            Assert.AreEqual(HttpStatusCodeDecision.Continue, Unit.HttpStatusCodeDecision);
            Assert.IsNotNull(HttpClient.DefaultRequestHeaders.Authorization);
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<TestType>(actual);
        }

        [Test]
        public async Task ItShouldDownloadTypeByUrlSuccessfully()
        {
            MockHttpMessageHandler
                .When(TestUrlMatch)
                .Respond(HttpStatusCode.OK, "application/json", ValidTestResponseData);

            var actual = await Unit.Download<TestType>(TestUrl);

            Assert.IsNull(Unit.LastException);
            Assert.IsNotNull(Unit.LastContent);
            Assert.AreEqual(HttpStatusCode.OK, Unit.LastCode);
            Assert.AreEqual(HttpStatusCodeDecision.Continue, Unit.HttpStatusCodeDecision);
            Assert.IsNotNull(HttpClient.DefaultRequestHeaders.Authorization);
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<TestType>(actual);
        }

        [Test]
        public async Task ItShouldUploadByUriAndFormDataSuccessfully()
        {
            MockHttpMessageHandler
                .When(TestUrlMatch)
                .Respond(HttpStatusCode.OK, "application/json", ValidTestResponseData);

            var formData = new Dictionary<string, string> {{"key", "value"}};
            var actual = await Unit.Upload<TestType>(TestUri, formData);


            Assert.IsNull(Unit.LastException);
            Assert.IsNotNull(Unit.LastContent);
            Assert.AreEqual(HttpStatusCode.OK, Unit.LastCode);
            Assert.AreEqual(HttpStatusCodeDecision.Continue, Unit.HttpStatusCodeDecision);
            Assert.IsNotNull(HttpClient.DefaultRequestHeaders.Authorization);
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<TestType>(actual);
        }
    }
}