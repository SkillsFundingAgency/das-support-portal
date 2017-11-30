using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using NUnit.Framework;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers
{
    public class WhenTestingAnMvcControllerOfType<T> where T : Controller
    {
        protected T Unit;
        protected ActionResult ActionResultResponse;
        protected ControllerContext UnitControllerContext;
        protected Mock<HttpContextBase> MockHttpContext;
        protected Mock<HttpRequestBase> MockHttpRequest;

        [SetUp]
        public virtual void Setup()
        {
            Unit = System.Activator.CreateInstance<T>();

            MockHttpContext = new Mock<HttpContextBase>();
            MockHttpRequest = new Mock<HttpRequestBase>();
            MockHttpContext.SetupGet(x => x.Request).Returns(MockHttpRequest.Object);
            MockHttpRequest.SetupGet(x => x.HttpMethod).Returns("GET");
            UnitControllerContext = new ControllerContext(MockHttpContext.Object, new RouteData(), Unit);

            Unit.ControllerContext = UnitControllerContext;

        }
    }
}
