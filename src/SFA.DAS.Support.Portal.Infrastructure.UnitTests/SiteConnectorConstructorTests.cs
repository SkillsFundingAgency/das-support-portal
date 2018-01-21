using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Shared;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Portal.Infrastructure.UnitTests
{
    [TestFixture]
    public class SiteConnectorConstructorTests
    {
        protected Mock<IClientAuthenticator> MockIClientAuthenticator;
        protected Mock<ISiteConnectorSettings> MockSiteConnectorSettings;
        protected Mock<ILog> MockLogger;

        [SetUp]
        public void Setup()
        {
            MockIClientAuthenticator = new Mock<IClientAuthenticator>();
            MockSiteConnectorSettings = new Mock<ISiteConnectorSettings>();
            MockLogger = new Mock<ILog>();
        }

        [Test]
        public void ItShouldThrowAnArgumentNullExceptionIfPassedANullHttpClient()
        {
            Assert.Throws<ArgumentNullException>(() =>
                 new SiteConnector(null, MockIClientAuthenticator.Object, MockSiteConnectorSettings.Object, MockLogger.Object)
            );
        }

        [Test]
        public void ItShouldThrowAnArgumentNullExceptionIfPassedANullSiteConnector()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SiteConnector(null, MockIClientAuthenticator.Object,null, MockLogger.Object)
            );
        }

        [Test]
        public void ItShouldThrowAnArgumentNullExceptionIfPassedANullClienAuthenticator()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SiteConnector(null, null ,MockSiteConnectorSettings.Object, MockLogger.Object)
            );
        }
        [Test]
        public void ItShouldThrowAnArgumentNullExceptionIfPassedANullLogger()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SiteConnector(null, MockIClientAuthenticator.Object ,MockSiteConnectorSettings.Object, null)
            );
        }
    }
}