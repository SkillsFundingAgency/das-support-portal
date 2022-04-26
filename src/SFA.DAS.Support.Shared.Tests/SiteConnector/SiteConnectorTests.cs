using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Shared.Tests.SiteConnector
{
    [TestFixture]
    public class SiteConnectorTests : SiteConnectorTestBase
    {
        [TestCase(HttpStatusCode.BadRequest)] // 400
        [TestCase(HttpStatusCode.Conflict)] // 409
        [TestCase(HttpStatusCode.ExpectationFailed)] // 417
        [TestCase(HttpStatusCode.RequestedRangeNotSatisfiable)] // 416
        [TestCase(HttpStatusCode.BadGateway)] // 502
        public async Task ItShouldReturnNullWhenDownloadTypeRecievesHttpStatus(HttpStatusCode code)
        {
            MockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) => new HttpResponseMessage(code) { Content = new StringContent(ValidTestResponseData) });

            var actual = await Unit.Download<TestType>(TestUri);
            Assert.IsNull(Unit.LastException);
            Assert.IsNotNull(Unit.LastContent);

            Assert.AreEqual(code, Unit.LastCode);
            Assert.AreEqual(HttpStatusCodeDecision.HandleException, Unit.HttpStatusCodeDecision);
            //Assert.IsNotNull(HttpClient.DefaultRequestHeaders.Authorization);
            Assert.IsNull(actual);
        }

        [TestCase(HttpStatusCode.Unauthorized)] // 401
        public async Task ItShouldReturnNullAndStripTheAuthorizationAfterThisCodeIsRecieved(HttpStatusCode code)
        {
            MockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) => new HttpResponseMessage(code) { Content = new StringContent(ValidTestResponseData) });

            var actual = await Unit.Download(new Uri(TestUrlMatch));
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
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) => 
                {
                    return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(ValidTestResponseData) };
                });

            var actual = await Unit.Download(new Uri(TestUrl));

            Assert.IsNull(Unit.LastException);
            Assert.IsNotNull(Unit.LastContent);
            Assert.AreEqual(HttpStatusCode.OK, Unit.LastCode);
            Assert.AreEqual(HttpStatusCodeDecision.Continue, Unit.HttpStatusCodeDecision);
            //Assert.IsNotNull(HttpClient.DefaultRequestHeaders.Authorization);
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<string>(actual);
        }

        [Test]
        public async Task ItShouldDownloadTypeByUriSuccessfully()
        {
            MockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(ValidTestResponseData) };
                });

            var actual = await Unit.Download<TestType>(TestUri);


            Assert.IsNull(Unit.LastException);
            Assert.IsNotNull(Unit.LastContent);
            Assert.AreEqual(HttpStatusCode.OK, Unit.LastCode);
            Assert.AreEqual(HttpStatusCodeDecision.Continue, Unit.HttpStatusCodeDecision);
           // Assert.IsNotNull(HttpClient.DefaultRequestHeaders.Authorization);
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<TestType>(actual);
        }

        [Test]
        public async Task ItShouldDownloadTypeByUrlSuccessfully()
        {
            MockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(ValidTestResponseData) };
                });

            var actual = await Unit.Download<TestType>(TestUrl);

            Assert.IsNull(Unit.LastException);
            Assert.IsNotNull(Unit.LastContent);
            Assert.AreEqual(HttpStatusCode.OK, Unit.LastCode);
            Assert.AreEqual(HttpStatusCodeDecision.Continue, Unit.HttpStatusCodeDecision);
           // Assert.IsNotNull(HttpClient.DefaultRequestHeaders.Authorization);
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<TestType>(actual);
        }

        [Test]
        public async Task ItShouldUploadByUriAndFormDataSuccessfully()
        {
            MockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(ValidTestResponseData) };
                });

            var formData = new Dictionary<string, string> {{"key", "value"}};
            var actual = await Unit.Upload<TestType>(TestUri, formData);


            Assert.IsNull(Unit.LastException);
            Assert.IsNotNull(Unit.LastContent);
            Assert.AreEqual(HttpStatusCode.OK, Unit.LastCode);
            Assert.AreEqual(HttpStatusCodeDecision.Continue, Unit.HttpStatusCodeDecision);
          //  Assert.IsNotNull(HttpClient.DefaultRequestHeaders.Authorization);
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<TestType>(actual);
        }
    }
}