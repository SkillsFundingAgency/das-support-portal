using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Shared.Authentication;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Shared.Tests
{

    [TestFixture]
    public class UnAuthorisedSiteConnectorTests
    {
        protected Mock<IClientAuthenticator> MockClientAuthenticator;
        protected Mock<ISiteConnectorSettings> MockSiteConnectorSettings;
        protected Mock<ILog> MockLogger;

        [SetUp]
        public void Setup()
        {
            MockClientAuthenticator = new Mock<IClientAuthenticator>();
            MockSiteConnectorSettings = new Mock<ISiteConnectorSettings>();
            MockLogger = new Mock<ILog>();
            _emptyJsonContent = "{}";
            _testType = new TestType();
            _validTestResponseData = JsonConvert.SerializeObject(_testType);
            _mockHttpMessageHandler = new MockHttpMessageHandler();
            _httpClient = new HttpClient(_mockHttpMessageHandler);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "dummytoken");

            _testUrlMatch = "http://localhost/api/user/*";
            _testUrl = "http://localhost/api/user/1234";
            _testUri = new Uri(_testUrl);

            _unit = new SiteConnector(_httpClient, MockClientAuthenticator.Object, MockSiteConnectorSettings.Object, MockLogger.Object);


            MockClientAuthenticator.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => "mockToken_dndndndndndndndnd=");

        }

        [TearDown]
        public void Teardown()
        {

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

        [Test]
        public async Task ItShouldRequestATokenForUnAuthorisedClients()
        {
            var code = HttpStatusCode.OK;
            _mockHttpMessageHandler
                .When(_testUrl)
                .Respond(code, "application/json", _validTestResponseData);

            var response = await _unit.Download(_testUrl);

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<string>(response);
            Assert.IsNotNull(_httpClient.DefaultRequestHeaders.Authorization);
        }

    }

    [TestFixture]
    public class SiteConnectorTests
    {
        protected Mock<IClientAuthenticator> MockClientAuthenticator;
        protected Mock<ISiteConnectorSettings> MockSiteConnectorSettings;
        protected Mock<ILog> MockLogger;

        [SetUp]
        public void Setup()
        {
            MockClientAuthenticator = new Mock<IClientAuthenticator>();
            MockSiteConnectorSettings = new Mock<ISiteConnectorSettings>();
            MockLogger = new Mock<ILog>();
            _emptyJsonContent = "{}";
            _testType = new TestType();
            _validTestResponseData = JsonConvert.SerializeObject(_testType);
            _mockHttpMessageHandler = new MockHttpMessageHandler();
            _httpClient = new HttpClient(_mockHttpMessageHandler);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "dummytoken");

            _testUrlMatch = "http://localhost/api/user/*";
            _testUrl = "http://localhost/api/user/1234";
            _testUri = new Uri(_testUrl);

            _unit = new SiteConnector(_httpClient, MockClientAuthenticator.Object, MockSiteConnectorSettings.Object, MockLogger.Object);


            MockClientAuthenticator.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => "mockToken_dndndndndndndndnd=");

        }

        [TearDown]
        public void Teardown()
        {

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
        [TestCase(HttpStatusCode.Conflict)] // 409
        [TestCase(HttpStatusCode.ExpectationFailed)] // 417
        [TestCase(HttpStatusCode.RequestedRangeNotSatisfiable)] // 416
        [TestCase(HttpStatusCode.BadGateway)] // 502
        public void ItShouldLogAndThrowExceptionWhenDownloadTypeRecievesHttpStatus(HttpStatusCode code)
        {

            _mockHttpMessageHandler
                .When(_testUrlMatch)
                .Respond(code, "application/json", _validTestResponseData)
                ;

            Assert.ThrowsAsync<HttpRequestException>(() => _unit.Download<TestType>(_testUri));
            Assert.IsNotNull(_unit.LastException);
            Assert.AreEqual(code, _unit.LastCode);
            MockLogger.Verify(x => x.Error(It.IsAny<Exception>(), It.IsAny<string>()));
            Assert.IsNotNull(_httpClient.DefaultRequestHeaders.Authorization);
        }
        
        [TestCase(HttpStatusCode.Unauthorized)] // 401
        public void ItShouldLogAndRememberExceptionThenStripTheAuthorizationAfterThisCodeIsRecieved(HttpStatusCode code)
        {
            _mockHttpMessageHandler
                .When(_testUrlMatch)
                .Respond(code, "application/json", "{}")
                ;

            Assert.DoesNotThrowAsync(() => _unit.Download(_testUrlMatch));
            MockLogger.Verify(x => x.Error(It.IsAny<Exception>(), It.IsAny<string>()));
            Assert.IsNotNull(_unit.LastException);
            Assert.AreEqual(code, _unit.LastCode);
            Assert.IsNull(_httpClient.DefaultRequestHeaders.Authorization);
        }

        [Test]
        public async Task ItShouldDownloadStringSuccessfully()
        {
            _mockHttpMessageHandler
                .When(_testUrl)
                .Respond(HttpStatusCode.OK, "application/json", _validTestResponseData);

            var response = await _unit.Download(_testUrl);
            Assert.IsNotNull(response);
            Assert.IsInstanceOf<string>(response);
            Assert.IsNotNull(_httpClient.DefaultRequestHeaders.Authorization);
        }

        [Test]
        public async Task ItShouldDownloadTypeByUriSuccessfully()
        {
            _mockHttpMessageHandler
                .When(_testUrl)
                .Respond(HttpStatusCode.OK, "application/json", _validTestResponseData);

            var response = await _unit.Download<TestType>(_testUri);

            Assert.IsNotNull(response);
            Assert.IsNotNull(_httpClient.DefaultRequestHeaders.Authorization);
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
            Assert.IsNotNull(_httpClient.DefaultRequestHeaders.Authorization);
        }

        [Test]
        public async Task ItShouldUploadByUriAndFormDataSuccessfully()
        {
            _mockHttpMessageHandler
                .When(_testUrlMatch)
                .Respond(HttpStatusCode.OK, "application/json", _validTestResponseData);

            var formData = new Dictionary<string, string> { { "key", "value" } };
            var response = await _unit.Upload<TestType>(_testUri, formData);

            Assert.IsNotNull(response);
            Assert.IsNotNull(_httpClient.DefaultRequestHeaders.Authorization);
        }
    }
}