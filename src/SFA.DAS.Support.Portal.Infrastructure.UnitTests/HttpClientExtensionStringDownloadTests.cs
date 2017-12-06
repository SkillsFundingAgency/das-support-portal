using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using System.Web;

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