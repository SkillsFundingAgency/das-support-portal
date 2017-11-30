using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.Web.Controllers;
using SFA.DAS.Support.Shared;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers
{
    public class WhenCallingTheVersionControllerManifestMethod : WithAPreparedVersionController
    {
        

        [SetUp]
        public override void Setup()
        {
            MockManifestRepository = new Mock<IManifestRepository>();
            MockLogger = new Mock<NLog.Logger.ILog>();
            Unit = new VersionController(MockManifestRepository.Object, MockLogger.Object);
        }

        [Test]
        public void ItShouldReturnTheKnownSiteManifests()
        {
            var siteManifests = new List<SiteManifest>(){new SiteManifest(){BaseUrl = "https://somewhere", Version = "99.99.99.99"}};
            MockManifestRepository.SetupGet(r => r.Manifests).Returns(siteManifests);
            var actual = Unit.Manifests();
            Assert.IsInstanceOf<ICollection<SiteManifest>>(actual);
            Assert.AreEqual(siteManifests, actual);
        }

    }
}