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
        public void ItShouldThrowAnArgumentNullExceptionIfPassedANullHttpClient()
        {
            Assert.Throws<ArgumentNullException>(() => new SiteConnector(null));
        }
        [Ignore("Will be re-applied in issue ASCS-57")]
        [Test]
        public void ItShouldThrowAnArgumentExceptionIfPassedAnHttpClientWithoutAnAuthorizationHeader()
        {
            Assert.Throws<ArgumentException>(() => new SiteConnector(new HttpClient()));
        }
    }
}