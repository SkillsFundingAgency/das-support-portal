using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Shared.Authentication;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Shared.Tests.SiteConnector
{
    public class SiteConnectorTestBase
    {
        protected static string TestUrl = "http://localhost/api/user/1234";
        protected string EmptyJsonContent;
        protected List<IHttpStatusCodeStrategy> Handlers;
        protected HttpClient HttpClient;
        protected Mock<IClientAuthenticator> MockClientAuthenticator;
        protected MockHttpMessageHandler MockHttpMessageHandler;
        protected Mock<ILog> MockLogger;
        protected Mock<ISiteConnectorSettings> MockSiteConnectorSettings;
        protected TestType TestType;
        protected Uri TestUri;
        protected string TestUrlMatch;
        protected ISiteConnector Unit;
        protected string ValidTestResponseData;

        [SetUp]
        public void Setup()
        {
            MockClientAuthenticator = new Mock<IClientAuthenticator>();
            MockSiteConnectorSettings = new Mock<ISiteConnectorSettings>();
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
            MockHttpMessageHandler = new MockHttpMessageHandler();
            HttpClient = new HttpClient(MockHttpMessageHandler);

            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "dummytoken");

            TestUrlMatch = "http://localhost/api/user/*";
            TestUrl = "http://localhost/api/user/1234";
            TestUri = new Uri(TestUrl);

            Unit = new SiteConnection.SiteConnector(HttpClient, MockClientAuthenticator.Object,
                MockSiteConnectorSettings.Object, Handlers, MockLogger.Object);


            MockClientAuthenticator.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => "mockToken_dndndndndndndndnd=");
        }

        [TearDown]
        public void Teardown()
        {
        }
    }
}