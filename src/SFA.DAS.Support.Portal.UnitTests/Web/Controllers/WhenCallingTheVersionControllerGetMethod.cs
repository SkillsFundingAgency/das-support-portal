using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.Web.Controllers;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers
{
    public class WhenCallingTheVersionControllerGetMethod : WithAPreparedVersionController
    {
      

        [SetUp]
        public override void Setup()
        {
            MockManifestRepository = new Mock<IManifestRepository>();
            MockLogger = new Mock<NLog.Logger.ILog>();
            Unit = new VersionController(MockManifestRepository.Object, MockLogger.Object);
        }

        [Test]
        public void ItShouldReturnVersionInformation()
        {
            var actual = Unit.Get();
            Assert.IsInstanceOf<VersionInformation>(actual);   
        }

    }
}