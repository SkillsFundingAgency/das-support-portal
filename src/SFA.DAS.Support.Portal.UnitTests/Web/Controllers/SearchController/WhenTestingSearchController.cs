using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.Web.Services;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.SearchController
{
    public abstract class WhenTestingSearchController
    {
        protected Mock<HttpContextBase> MockContextBase;
        protected Mock<ILog> MockLogger;
        protected Mock<IMappingService> MockMappingService;
        protected Mock<IMediator> MockMediator;
        protected Mock<HttpRequestBase> MockRequestBase;
        protected Mock<HttpResponseBase> MockResponseBase;
        protected Mock<IPrincipal> MockUser;
        protected RouteData RouteData;
        protected Portal.Web.Controllers.SearchController Unit;
        protected ControllerContext UnitControllerContext;


        [SetUp]
        public void Init()
        {
            MockLogger = new Mock<ILog>();
            MockMappingService = new Mock<IMappingService>();
            MockMediator = new Mock<IMediator>();

            Unit = new Portal.Web.Controllers.SearchController(
                MockMappingService.Object,
                MockMediator.Object);

            RouteData = new RouteData();
            MockContextBase = new Mock<HttpContextBase>();

            MockRequestBase = new Mock<HttpRequestBase>();
            MockResponseBase = new Mock<HttpResponseBase>();
            MockUser = new Mock<IPrincipal>();

            MockContextBase.Setup(x => x.Request).Returns(MockRequestBase.Object);
            MockContextBase.Setup(x => x.Response).Returns(MockResponseBase.Object);
            MockContextBase.Setup(x => x.User).Returns(MockUser.Object);
            UnitControllerContext = new ControllerContext(MockContextBase.Object, RouteData, Unit);

            Unit.ControllerContext = UnitControllerContext;
        }
    }
}