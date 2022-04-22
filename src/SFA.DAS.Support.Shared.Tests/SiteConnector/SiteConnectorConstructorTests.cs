using System;
using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Shared.Tests.SiteConnector
{
    [TestFixture]
    public class SiteConnectorConstructorTests : SiteConnectorTestBase
    {
        [Test]
        public void ItShouldThrowAnArgumentExceptionIfPassedZeroHandlers()
        {
            Handlers = new List<IHttpStatusCodeStrategy>();
            Assert.Throws<ArgumentNullException>(() =>
                new SiteConnection.SiteConnector(null, MockClientAuthenticator.Object, MockSiteConnectorSettings.Object, MockIEmployerAccountSiteConnectorSettings.Object,
                    MockISupportCommitmentsSiteConnectorSettings.Object, MockISupportEmployerUsersSiteConnectorSettings.Object,
                    Handlers, MockLogger.Object)
            );
        }

        [Test]
        public void ItShouldThrowAnArgumentNullExceptionIfPassedANullClienAuthenticator()
        {
            Handlers = new List<IHttpStatusCodeStrategy>();
            Assert.Throws<ArgumentNullException>(() =>
                new SiteConnection.SiteConnector(null, null, MockSiteConnectorSettings.Object, MockIEmployerAccountSiteConnectorSettings.Object,
                MockISupportCommitmentsSiteConnectorSettings.Object, MockISupportEmployerUsersSiteConnectorSettings.Object, Handlers,
                    MockLogger.Object)
            );
        }

        [Test]
        public void ItShouldThrowAnArgumentNullExceptionIfPassedANullHttpClient()
        {
            Handlers = new List<IHttpStatusCodeStrategy>();
            Assert.Throws<ArgumentNullException>(() =>
                new SiteConnection.SiteConnector(null, MockClientAuthenticator.Object, MockSiteConnectorSettings.Object, MockIEmployerAccountSiteConnectorSettings.Object,
                MockISupportCommitmentsSiteConnectorSettings.Object, MockISupportEmployerUsersSiteConnectorSettings.Object,
                    Handlers, MockLogger.Object)
            );
        }

        [Test]
        public void ItShouldThrowAnArgumentNullExceptionIfPassedANullLogger()
        {
            Handlers = new List<IHttpStatusCodeStrategy>();
            Assert.Throws<ArgumentNullException>(() =>
                new SiteConnection.SiteConnector(null, MockClientAuthenticator.Object, MockSiteConnectorSettings.Object, MockIEmployerAccountSiteConnectorSettings.Object,
                MockISupportCommitmentsSiteConnectorSettings.Object, MockISupportEmployerUsersSiteConnectorSettings.Object,
                    Handlers, null)
            );
        }

        [Test]
        public void ItShouldThrowAnArgumentNullExceptionIfPassedANullSiteConnector()
        {
            Handlers = new List<IHttpStatusCodeStrategy>();
            Assert.Throws<ArgumentNullException>(() =>
                new SiteConnection.SiteConnector(null, MockClientAuthenticator.Object, null, null, null, null, Handlers,
                    MockLogger.Object)
            );
        }
    }
}