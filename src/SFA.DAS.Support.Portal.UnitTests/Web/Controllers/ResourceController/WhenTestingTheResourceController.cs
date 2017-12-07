using System.IO;
using System.Security.Principal;
using System.Web;
using System.Web.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.Web.Services;
using System.Web.Mvc;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.ResourceController
{
    public abstract class WhenTestingTheResourceController : 
        WhenTestingAnMvcControllerOfType<Portal.Web.Controllers.ResourceController>
    {
        protected Mock<IManifestRepository> MockManifestRepository;
        protected Mock<ICheckPermissions> MockPermissionsChecker;
        protected Mock<IGrantPermissions> MockPermissionsGranter;
        protected Mock<HttpContextBase> MockContextBase;
        protected ControllerContext UnitControllerContext;

        [SetUp]
        public override void Setup()
        {
            MockManifestRepository = new Mock<IManifestRepository>();
            MockPermissionsChecker = new Mock<ICheckPermissions>();
            MockPermissionsGranter = new Mock<IGrantPermissions>();

            Unit = new Portal.Web.Controllers.ResourceController(
                MockManifestRepository.Object, 
                MockPermissionsChecker.Object, 
                MockPermissionsGranter.Object);

            MockContextBase = new Mock<HttpContextBase>();

            MockContextBase.Setup(x => x.Request).Returns(new Mock<HttpRequestBase>().Object);
            MockContextBase.Setup(x => x.Response).Returns(new Mock<HttpResponseBase>().Object);
            MockContextBase.Setup(x => x.User).Returns(new Mock<IPrincipal>().Object);
           
            UnitControllerContext = new ControllerContext(MockContextBase.Object, new RouteData(), Unit );

            Unit.ControllerContext = UnitControllerContext;

            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter())
            );
        }
    }
}
