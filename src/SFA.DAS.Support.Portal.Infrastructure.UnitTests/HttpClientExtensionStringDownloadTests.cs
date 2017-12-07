using System;
using System.Net.Http;
using NUnit.Framework;
using RichardSzalay.MockHttp;

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
  }
}