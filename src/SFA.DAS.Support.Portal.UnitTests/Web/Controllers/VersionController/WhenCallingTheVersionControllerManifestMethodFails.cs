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
            
            MockManifestRepository.Setup(r => r.GetManifests()).Throws<System.Exception>();
            Assert.That(async () => await Unit.Manifests(), Throws.Exception.TypeOf<System.Exception>());

        }
        [Test]
        public async Task ItShouldLogTheException()
        {

            MockManifestRepository.Setup(r => r.GetManifests()).Throws<System.Exception>();
            Assert.That(async () => await Unit.Manifests(), Throws.Exception.TypeOf<System.Exception>());
            MockLogger.Verify(l=> l.Error(It.IsAny<System.Exception>(),  It.IsAny<string>()));
        }
    }
}