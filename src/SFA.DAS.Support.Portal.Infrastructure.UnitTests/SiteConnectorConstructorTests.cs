using System;
using System.Net.Http;
using NUnit.Framework;
using SFA.DAS.Support.Portal.Infrastructure.Services;

namespace SFA.DAS.Support.Portal.Infrastructure.UnitTests
{
    [TestFixture]
    public class SiteConnectorConstructorTests
    {
        [Test]
        public void ItShouldThrowAnArgumentExceptionIfPassedANullHttpClient()
        {
            Assert.Throws<ArgumentException>(() => new SiteConnector(null, null, null));
        }

        [Test]
        public void ItShouldThrowAnArgumentExceptionIfPassedANullClientAuthenticator()
        {
            Assert.Throws<ArgumentException>(() => new SiteConnector(new HttpClient(), null, null));
        }


        [Test]
        public void ItShouldThrowAnArgumentExceptionIfPassedANullSettings()
        {
            Assert.Throws<ArgumentException>(() => new SiteConnector(new HttpClient(), null, null));
        }
    }
}