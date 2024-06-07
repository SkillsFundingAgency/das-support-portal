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
                new SiteConnection.SiteConnector(null,
                    Handlers, MockLogger.Object,
                     MockazureClientCredentialHelper.Object)
            );
        }

        [Test]
        public void ItShouldThrowAnArgumentNullExceptionIfPassedANullClienAuthenticator()
        {
            Handlers = new List<IHttpStatusCodeStrategy>();
            Assert.Throws<ArgumentNullException>(() =>
                new SiteConnection.SiteConnector(null, Handlers,
                    MockLogger.Object,
                    MockazureClientCredentialHelper.Object)
            );
        }

        [Test]
        public void ItShouldThrowAnArgumentNullExceptionIfPassedANullHttpClient()
        {
            Handlers = new List<IHttpStatusCodeStrategy>();
            Assert.Throws<ArgumentNullException>(() =>
                new SiteConnection.SiteConnector(null,
                    Handlers, MockLogger.Object,
                    MockazureClientCredentialHelper.Object)
            );
        }

        [Test]
        public void ItShouldThrowAnArgumentNullExceptionIfPassedANullLogger()
        {
            Handlers = new List<IHttpStatusCodeStrategy>();
            Assert.Throws<ArgumentNullException>(() =>
                new SiteConnection.SiteConnector(null,
                    Handlers, null,
                    MockazureClientCredentialHelper.Object)
            );
        }

        [Test]
        public void ItShouldThrowAnArgumentNullExceptionIfPassedANullSiteConnector()
        {
            Handlers = new List<IHttpStatusCodeStrategy>();
            Assert.Throws<ArgumentNullException>(() =>
                new SiteConnection.SiteConnector(null,
                   Handlers,
                    MockLogger.Object,
                    MockazureClientCredentialHelper.Object)
            );
        }
    }
}