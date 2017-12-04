using Moq;
using NUnit.Framework;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.Version
{
    public class WhenCallingTheVersionControllerManifestMethodFails : WithAPreparedVersionController
    {

        [Test]
        public void ItShouldThrowAnException()
        {
            
            MockManifestRepository.SetupGet(r => r.Manifests).Throws<System.Exception>();
            Assert.Throws<System.Exception>(() => Unit.Manifests());
            
        }
        [Test]
        public void ItShouldLogTheException()
        {

            MockManifestRepository.SetupGet(r => r.Manifests).Throws<System.Exception>();
            Assert.Throws<System.Exception>(()=> Unit.Manifests());
            MockLogger.Verify(l=> l.Error(It.IsAny<System.Exception>(),  It.IsAny<string>()));
            

        }
    }
}