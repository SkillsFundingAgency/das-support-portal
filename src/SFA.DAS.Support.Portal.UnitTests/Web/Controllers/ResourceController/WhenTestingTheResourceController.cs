using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.Web.Services;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.ResourceController
{
    public abstract class WhenTestingTheResourceController :
        WhenTestingAnMvcControllerOfType<Portal.Web.Controllers.ResourceController>
    {
        protected Mock<HttpContextBase> MockContextBase;
        protected Mock<IManifestRepository> MockManifestRepository;
        protected Mock<ICheckPermissions> MockPermissionsChecker;
        protected Mock<IGrantPermissions> MockPermissionsGranter;

        protected IServiceConfiguration ServiceConfiguration;
        protected ControllerContext UnitControllerContext;

        [SetUp]
        public override void Setup()
        {
            ServiceConfiguration = new ServiceConfiguration { new EmployerAccountSiteManifest() };

            MockManifestRepository = new Mock<IManifestRepository>();
            MockPermissionsChecker = new Mock<ICheckPermissions>();
            MockPermissionsGranter = new Mock<IGrantPermissions>();

            Unit = new Portal.Web.Controllers.ResourceController(
                MockManifestRepository.Object,
                MockPermissionsChecker.Object,
                MockPermissionsGranter.Object,
                ServiceConfiguration
                , Mock.Of<ILog>());

            MockContextBase = new Mock<HttpContextBase>();

            MockContextBase.Setup(x => x.Request).Returns(new Mock<HttpRequestBase>().Object);
            MockContextBase.Setup(x => x.Response).Returns(new Mock<HttpResponseBase>().Object);
            MockContextBase.Setup(x => x.User).Returns(new Mock<IPrincipal>().Object);

            UnitControllerContext = new ControllerContext(MockContextBase.Object, new RouteData(), Unit);

            Unit.ControllerContext = UnitControllerContext;
        }
    }
}