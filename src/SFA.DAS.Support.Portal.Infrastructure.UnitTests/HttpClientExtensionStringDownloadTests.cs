using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using SFA.DAS.Support.Portal.Infrastructure.Extensions;
using System.Web;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Support.Portal.Infrastructure.UnitTests
{
    [TestFixture]
    public class HttpClientExtensionStringDownloadTests
    {
        private MockHttpMessageHandler _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private Uri _testUri = new Uri("http://localost/api/user/1234");
        private string _validTestResponseData;
        private string _emptyJsonContent;

        [SetUp]
        public void Setup()
        {
            _emptyJsonContent = "{}";
            _validTestResponseData = "{'name' : 'Test McGee'}";


            _mockHttpMessageHandler = new MockHttpMessageHandler();
            _httpClient = new HttpClient(_mockHttpMessageHandler);
        }

        [Test]
        public async Task ItShouldDownloadJsonSuccessfully()
        {
            // Arrange
            _mockHttpMessageHandler
               .When("http://localost/api/user/*")
               .Respond(HttpStatusCode.OK, "application/json", _validTestResponseData);

            // Act
            var response = await _httpClient.Download(_testUri);

            Assert.IsNotNull(response);
        }


        [Test]
        public async Task ItShouldNotThrowAnExceptionWhenDownloadReturnsStatus400()
        {
            // Arrange
            _mockHttpMessageHandler
                .When("http://localost/api/user/*")
                .Respond(HttpStatusCode.Unauthorized, "application/json", "")
                ;

            try
            {
                var response = await _httpClient.Download(_testUri);
            }
            catch (HttpException e)
            {
                Assert.Pass("Exception recieved");
            }
            catch (Exception e)
            {
                Assert.Fail("Expected Exception not thrown");

            }
            Assert.Fail("No Exception thrown");
            // Act

        }



        [TestCase(HttpStatusCode.BadRequest)]// 400
        public async Task ItShouldNotThrowAnExceptionWhenDownloadRecievesHttpStatus(HttpStatusCode code)
        {
            // Arrange
            _mockHttpMessageHandler
                .When("http://localost/api/user/*")
                .Respond(code, "application/json", _emptyJsonContent)
                ;

            try
            {
                var response = await _httpClient.Download(_testUri);

            }
            catch (HttpException e)
            {
                Assert.Pass("Exception recieved");
            }
            catch (Exception e)
            {
                Assert.Fail("Expected Exception not thrown");
            }
            Assert.Fail("No Exception was thrown");
            // Act

        }

        [TestCase(HttpStatusCode.Unauthorized)] // 401
        [TestCase(HttpStatusCode.Conflict)] // 409
        [TestCase(HttpStatusCode.ExpectationFailed)] // 417
        [TestCase(HttpStatusCode.RequestedRangeNotSatisfiable)] // 416
        [TestCase(HttpStatusCode.BadGateway)] // 502
        public async Task ItShouldThrowAnExceptionWhenDownloadRecievesHttpStatus(HttpStatusCode code)
        {
            // Arrange
            _mockHttpMessageHandler
                .When("http://localost/api/user/*")
                .Respond(code, "application/json", "{}")
                ;

            try
            {
                var response = await _httpClient.Download(_testUri);

            }
            catch (HttpException e)
            {
                Assert.Pass("Exception recieved");
            }
            catch (Exception e)
            {
                Assert.Fail("Expected Exception not thrown");

            }
            Assert.Fail("No Exception was thrown");
            // Act

        }


    }
}