using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.VersionController
{
    public class WhenCallingTheVersionControllerManifestMethodFails : WithAPreparedVersionController
    {
        [Test]
        public void ItShouldThrowAnException()
        {
            MockManifestRepository.Setup(r => r.GetManifests()).Throws<Exception>();
            Assert.That(async () => await Unit.Manifests(), Throws.Exception.TypeOf<Exception>());
        }

        [Test]
        public async Task ItShouldLogTheException()
        {
            MockManifestRepository.Setup(r => r.GetManifests()).Throws<Exception>();
            Assert.That(async () => await Unit.Manifests(), Throws.Exception.TypeOf<Exception>());
            MockLogger.Verify(l => l.Error(It.IsAny<Exception>(), It.IsAny<string>()));
        }
    }
}