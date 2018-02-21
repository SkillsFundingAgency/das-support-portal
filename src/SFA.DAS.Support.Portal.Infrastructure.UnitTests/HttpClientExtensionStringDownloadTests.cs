using System;
using System.Net;
using System.Net.Http;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace SFA.DAS.Support.Portal.Infrastructure.UnitTests
{
    [TestFixture]
    public class HttpClientExtensionStringDownloadTests
    {
        [SetUp]
        public void Setup()
        {
            _emptyJsonContent = "{}";
           

            _mockHttpMessageHandler = new MockHttpMessageHandler();
            _httpClient = new HttpClient(_mockHttpMessageHandler);
        }

        private MockHttpMessageHandler _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private Uri _testUri = new Uri("http://localost/api/user/1234");
        private string _emptyJsonContent;

        [Test]
        public void ItShoudMockAGetOperation()
        {
            _mockHttpMessageHandler.When("http://localhost/api/user/1234")
                .Respond("application/json", _emptyJsonContent);

            var result = _httpClient.GetAsync(new Uri("http://localhost/api/user/1234")).Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(_emptyJsonContent, result.Content.ReadAsStringAsync().Result);
        }
    }
}