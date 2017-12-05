using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using SFA.DAS.Support.Portal.Infrastructure.Extensions;

namespace SFA.DAS.Support.Portal.Infrastructure.UnitTests
{
    [TestFixture]
    public class HttpClientExtensionTypedDownloadAsTests
    {
        private MockHttpMessageHandler _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private Uri _testUri = new Uri("http://localost/api/user/1234");
        private string _validTestResponseData;
        private string _emptyJsonContent;
        private TestType _testType;

        [SetUp]
        public void Setup()
        {
            _emptyJsonContent = "{}";
            _testType = new TestType();
            _validTestResponseData = JsonConvert.SerializeObject(_testType);
            _mockHttpMessageHandler = new MockHttpMessageHandler();
            _httpClient = new HttpClient(_mockHttpMessageHandler);
        }

        [Test]
        public async Task ItShouldDownloadJsonAndDeserializeSuccessfully()
        {
            // Arrange
            _mockHttpMessageHandler
                .When("http://localost/api/user/*")
                .Respond(HttpStatusCode.OK, "application/json", _validTestResponseData);

            // Act
            var response = await _httpClient.DownloadAs<TestType>(_testUri);

            Assert.IsNotNull(response);

        }


        [TestCase(HttpStatusCode.Ambiguous)]// 300
        public async Task ItShouldNotReturnNullWhenDownloadRecievesHttpStatus(HttpStatusCode code)
        {
            // Arrange
            _mockHttpMessageHandler
                .When("http://localost/api/user/*")
                .Respond(code, "application/json", _validTestResponseData)
                ;

            TestType response = null;
            try
            {
                response = await _httpClient.DownloadAs<TestType>(_testUri);
            }
            catch (HttpException e)
            {
                Assert.Fail($"Exception of type {nameof(HttpException)} thrown");
            }
            catch (Exception e)
            {
                Assert.Fail($"Exception of type {nameof(Exception)} thrown");
            }
            Assert.IsNotNull(response);
        }

        [TestCase(HttpStatusCode.Unauthorized)] // 401
        [TestCase(HttpStatusCode.Conflict)] // 409
        [TestCase(HttpStatusCode.ExpectationFailed)] // 417
        [TestCase(HttpStatusCode.RequestedRangeNotSatisfiable)] // 416
        [TestCase(HttpStatusCode.BadGateway)] // 502
        public async Task ItShouldThrowreturnNullWhenDownloadRecievesHttpStatus(HttpStatusCode code)
        {
            // Arrange
            _mockHttpMessageHandler
                .When("http://localost/api/user/*")
                .Respond(code, "application/json", _validTestResponseData)
                ;
            TestType response = null;
            try
            {
                response = await _httpClient.DownloadAs<TestType>(_testUri);
            }
            catch (HttpException e)
            {
                Assert.Fail($"Exception of type {nameof(HttpException)} thrown");
            }
            catch (Exception e)
            {
                Assert.Fail($"Exception of type {nameof(Exception)} thrown");
            }
            Assert.IsNull(response);
        }
    }
}