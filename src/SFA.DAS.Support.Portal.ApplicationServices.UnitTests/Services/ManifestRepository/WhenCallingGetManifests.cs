using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Shared;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Services.ManifestRepository
{
    [TestFixture]
    public class WhenCallingGetManifests : WhenTestingManifestRepository
    {
        [TearDown]
        public void Teardown()
        {
            MockLogger.Verify(x => x.Debug($"Downloading '{TestSiteUri}'"), Times.Once);
        }

        [Test]
        public async Task ItShouldLogTheExceptionOnError()
        {
            MockSiteConnector.Setup(x => x.Download<SiteManifest>(It.IsAny<Uri>()))
                .ThrowsAsync(new HttpException());

            var result = await Unit.GetManifests();

            MockLogger.Verify(x=>x.Error(It.IsAny<HttpException>(), It.IsAny<string>()), Times.Once);
            CollectionAssert.IsEmpty(result);

        }

        [Test]
        public async Task ItShouldReturnAnEmptyListOnError()
        {
            MockSiteConnector.Setup(x => x.Download<SiteManifest>(It.IsAny<Uri>()))
                .ThrowsAsync(new HttpException());
            var result = await Unit.GetManifests();

            CollectionAssert.IsEmpty(result);
        }
        [Test]
        public async Task ItShouldReturnTheListOfManifestObjects()
        {
            var result = await Unit.GetManifests();

            CollectionAssert.IsNotEmpty(result);
        }
    }
}