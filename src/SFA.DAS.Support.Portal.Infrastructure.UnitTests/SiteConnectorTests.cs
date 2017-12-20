using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.Infrastructure.Services;

namespace SFA.DAS.Support.Portal.Infrastructure.UnitTests
{
    [TestFixture]
    public class SiteConnectorTests
    {
        [SetUp]
        public void Setup()
        {
            _emptyJsonContent = "{}";
            _testType = new TestType();
            _validTestResponseData = JsonConvert.SerializeObject(_testType);
            _mockHttpMessageHandler = new MockHttpMessageHandler();
            _httpClient = new HttpClient(_mockHttpMessageHandler);
            _testUrlMatch = "http://localhost/api/user/*";
            _testUrl = "http://localhost/api/user/1234";
            _testUri = new Uri(_testUrl);
            _unit = new SiteConnector(_httpClient);
        }

        private MockHttpMessageHandler _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private Uri _testUri;
        private string _validTestResponseData;
        private string _emptyJsonContent;
        private TestType _testType;
        private ISiteConnector _unit;
        private string _testUrlMatch;
        private static string _testUrl = "http://localhost/api/user/1234";


        [TestCase(HttpStatusCode.Ambiguous)] // 300
        [TestCase(HttpStatusCode.BadRequest)] // 400
        [TestCase(HttpStatusCode.Unauthorized)] // 401
        [TestCase(HttpStatusCode.Conflict)] // 409
        [TestCase(HttpStatusCode.ExpectationFailed)] // 417
        [TestCase(HttpStatusCode.RequestedRangeNotSatisfiable)] // 416
        [TestCase(HttpStatusCode.BadGateway)] // 502
        public void ItShouldThrowAnExceptionWhenDownloadTypeRecievesHttpStatus(HttpStatusCode code)
        {
            _mockHttpMessageHandler
                .When(_testUrlMatch)
                .Respond(code, "application/json", _validTestResponseData)
                ;

            Assert.ThrowsAsync<HttpRequestException>(() => _unit.Download<TestType>(_testUri));
        }

        [TestCase(HttpStatusCode.Ambiguous)] // 300
        [TestCase(HttpStatusCode.BadRequest)] // 400
        [TestCase(HttpStatusCode.Unauthorized)] // 401
        [TestCase(HttpStatusCode.Conflict)] // 409
        [TestCase(HttpStatusCode.ExpectationFailed)] // 417
        [TestCase(HttpStatusCode.RequestedRangeNotSatisfiable)] // 416
        [TestCase(HttpStatusCode.BadGateway)] // 502
        public async Task ItShouldThrowAnExceptionWhenDownloadStringRecievesHttpStatus(HttpStatusCode code)
        {
            _mockHttpMessageHandler
                .When(_testUrlMatch)
                .Respond(code, "application/json", "{}")
                ;

            Assert.ThrowsAsync<HttpRequestException>(() => _unit.Download(_testUrlMatch));
        }

        [Test]
        public async Task ItShouldDownloadStringSuccessfully()
        {
            _mockHttpMessageHandler
                .When(_testUrlMatch)
                .Respond(HttpStatusCode.OK, "application/json", _validTestResponseData);

            var response = await _unit.Download(_testUrl);
            Assert.IsNotNull(response);
            Assert.IsInstanceOf<string>(response);
        }

        [Test]
        public async Task ItShouldDownloadTypeByUriSuccessfully()
        {
            _mockHttpMessageHandler
                .When(_testUrlMatch)
                .Respond(HttpStatusCode.OK, "application/json", _validTestResponseData);

            var response = await _unit.Download<TestType>(_testUri);

            Assert.IsNotNull(response);
        }

        [Test]
        public async Task ItShouldDownloadTypeByUrlSuccessfully()
        {
            _mockHttpMessageHandler
                .When(_testUrlMatch)
                .Respond(HttpStatusCode.OK, "application/json", _validTestResponseData);

            var response = await _unit.Download<TestType>(_testUrl);
            Assert.IsNotNull(response);
            Assert.IsInstanceOf<TestType>(response);
        }

        [Test]
        public async Task ItShouldUploadByUriAndFormDataSuccessfully()
        {
            _mockHttpMessageHandler
                .When(_testUrlMatch)
                .Respond(HttpStatusCode.OK, "application/json", _validTestResponseData);

            var formData = new Dictionary<string, string> {{"key", "value"}};
            var response = await _unit.Upload<TestType>(_testUri, formData);

            Assert.IsNotNull(response);
        }
    }
}