using System.Security.Principal;
using System.Web;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.Infrastructure.Services;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.VersionController
{
    public class WithAPreparedVersionController : WhenTestingAnApiControllerOfType<Portal.Web.Controllers.VersionController>
    {
        protected Mock<IWindowsLogonIdentityProvider> WindowsLogonIdentityProvider;
        protected Mock<IManifestRepository> MockManifestRepository;
        protected Mock<NLog.Logger.ILog> MockLogger;
        protected HttpContextBase HttpContextBase;

        [SetUp]
        public override void Setup()
        {
            MockManifestRepository = new Mock<IManifestRepository>();
            MockLogger = new Mock<NLog.Logger.ILog>();
            WindowsLogonIdentityProvider = new Mock<IWindowsLogonIdentityProvider>();
            WindowsLogonIdentityProvider.Setup(x => x.GetIdentity()).Returns(WindowsIdentity.GetAnonymous);

            Unit = new Portal.Web.Controllers.VersionController(MockManifestRepository.Object, MockLogger.Object, WindowsLogonIdentityProvider.Object);
        }
    }
}