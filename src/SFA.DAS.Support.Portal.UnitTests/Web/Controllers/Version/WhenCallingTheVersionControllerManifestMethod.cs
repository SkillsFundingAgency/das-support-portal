using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.Web.Controllers;
using SFA.DAS.Support.Shared;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.Version
{
    public class WhenCallingTheVersionControllerManifestMethod : WithAPreparedVersionController
    {
       


        [Test]
        public async Task ItShouldReturnTheKnownSiteManifests()
        {
            var siteManifests = new List<SiteManifest>(){new SiteManifest(){BaseUrl = "https://somewhere", Version = "99.99.99.99"}};
            MockManifestRepository.Setup(r => r.GetManifests()).Returns(Task.FromResult(siteManifests));
            var actual = await Unit.Manifests();
            Assert.IsInstanceOf<List<SiteManifest>>(actual);
            Assert.AreEqual(siteManifests, actual);
        }

    }
}