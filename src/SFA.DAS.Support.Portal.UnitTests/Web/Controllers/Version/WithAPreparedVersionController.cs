using System.Security.Principal;
using System.Web;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.Web.Controllers;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.Version
{
    public class WithAPreparedVersionController : WhenTestingAnApiControllerOfType<VersionController>
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

            Unit = new VersionController(MockManifestRepository.Object, MockLogger.Object, WindowsLogonIdentityProvider.Object);
        }
    }
}