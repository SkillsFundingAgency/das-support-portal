using System;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Support.Portal.Infrastructure.Services;

namespace SFA.DAS.Support.Portal.Infrastructure.UnitTests
{
    [TestFixture]
    public class WebDownloaderconstructorTests
    {
        [Test]
        public async Task ItShouldThrowAnArgumentExceptionIfPassedANullHttpClient()
        {
            Assert.Throws<ArgumentException>(() => new SiteConnector(null));
        }
    }
}