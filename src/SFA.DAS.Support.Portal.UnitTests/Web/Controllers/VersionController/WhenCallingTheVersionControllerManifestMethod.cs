using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.VersionController
{
    public class WhenCallingTheVersionControllerManifestMethod : WithAPreparedVersionController
    {
        [Test]
        public async Task ItShouldReturnTheKnownSiteManifests()
        {
            var siteManifests =
                new List<SiteManifest> {new SiteManifest {BaseUrl = "https://somewhere", Version = "99.99.99.99"}};
            MockManifestRepository.Setup(r => r.GetManifests()).Returns(Task.FromResult(siteManifests));
            var actual = await Unit.Manifests();
            Assert.IsInstanceOf<List<SiteManifest>>(actual);
            Assert.AreEqual(siteManifests, actual);
        }
    }
}