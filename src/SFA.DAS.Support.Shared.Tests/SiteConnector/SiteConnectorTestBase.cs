using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Shared.Authentication;
using SFA.DAS.Support.Shared.SiteConnection;
using SFA.DAS.Support.Shared.SiteConnection.Authentication;

namespace SFA.DAS.Support.Shared.Tests.SiteConnector
{
    public class SiteConnectorTestBase
    {
        protected static string TestUrl;
        protected string EmptyJsonContent;
        protected List<IHttpStatusCodeStrategy> Handlers;
        protected HttpClient HttpClient;
        protected Mock<IClientAuthenticator> MockClientAuthenticator;
        protected Mock<ILog> MockLogger;
        protected TestType TestType;
        protected Uri TestUri;
        protected string TestResourceIdentifier;
        protected string TestUrlMatch;
        protected ISiteConnector Unit;
        protected string ValidTestResponseData;
        protected Mock<HttpMessageHandler> MockHttpMessageHandler;
        protected HttpRequestMessage HttpRequestMessage;
        protected Mock<IAzureClientCredentialHelper> MockazureClientCredentialHelper;

        [SetUp]
        public void Setup()
        {
            var configuration = new HttpConfiguration();
            HttpRequestMessage = new System.Net.Http.HttpRequestMessage();
            HttpRequestMessage.Properties[System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey] = configuration;

            MockClientAuthenticator = new Mock<IClientAuthenticator>();
            MockLogger = new Mock<ILog>();
            Handlers = new List<IHttpStatusCodeStrategy>
            {
                new StrategyForInformationStatusCode(MockLogger.Object),
                new StrategyForSuccessStatusCode(MockLogger.Object),
                new StrategyForRedirectionStatusCode(MockLogger.Object),
                new StrategyForClientErrorStatusCode(MockLogger.Object),
                new StrategyForSystemErrorStatusCode(MockLogger.Object)
            };
            EmptyJsonContent = "{}";
            TestType = new TestType();
            ValidTestResponseData = JsonConvert.SerializeObject(TestType);
            MockHttpMessageHandler = new Mock<HttpMessageHandler>();
            HttpClient = new HttpClient(MockHttpMessageHandler.Object);

            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "dummytoken");

            TestUrlMatch = "http://localhost/api/user/*";
            TestUrl = "http://localhost/api/user/1234";
            TestUri = new Uri(TestUrl);
            TestResourceIdentifier = "https://citizenazuresfabisgov.onmicrosoft.com/das-at-test-as-ar";

            MockazureClientCredentialHelper = new Mock<IAzureClientCredentialHelper>();
            MockazureClientCredentialHelper
                .Setup(x => x.GetAccessTokenAsync(It.IsAny<string>()))
                .ReturnsAsync("AzureToken");

            MockClientAuthenticator.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>(),
                  It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(() => "mockToken_dndndndndndndndnd=");

            Unit = new SiteConnection.SiteConnector(HttpClient,
                MockClientAuthenticator.Object,
                Handlers,
                MockLogger.Object,
                MockazureClientCredentialHelper.Object);
        }

        [TearDown]
        public void Teardown()
        {
        }
    }
}