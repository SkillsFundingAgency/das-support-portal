using System.Security.Principal;
using System.Web;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.Infrastructure.Services;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.VersionController
{
    public class
        WithAPreparedVersionController : WhenTestingAnApiControllerOfType<Portal.Web.Controllers.VersionController>
    {
        protected HttpContextBase HttpContextBase;
        protected Mock<ILog> MockLogger;
        protected Mock<IManifestRepository> MockManifestRepository;
        protected Mock<IWindowsLogonIdentityProvider> WindowsLogonIdentityProvider;

        [SetUp]
        public override void Setup()
        {
            MockManifestRepository = new Mock<IManifestRepository>();
            MockLogger = new Mock<ILog>();
            WindowsLogonIdentityProvider = new Mock<IWindowsLogonIdentityProvider>();
            WindowsLogonIdentityProvider.Setup(x => x.GetIdentity()).Returns(WindowsIdentity.GetAnonymous);

            Unit = new Portal.Web.Controllers.VersionController(MockManifestRepository.Object, MockLogger.Object,
                WindowsLogonIdentityProvider.Object);
        }
    }
}