using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices;
using SFA.DAS.Support.Portal.Web.Services;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.ChallengeController
{
    public abstract class
        WhenTestingTheChallengeController : WhenTestingAnMvcControllerOfType<Portal.Web.Controllers.ChallengeController>
    {
        protected Mock<IAccountRepository> AccountRepository;
        protected Mock<HttpContextBase> MockContextBase;
        protected Mock<IGrantPermissions> MockGranter;
        protected Mock<IMediator> MockMediator;
        protected Mock<HttpResponseBase> MockResponse;
        protected Mock<IPrincipal> MockUser;
        protected ControllerContext UnitControllerContext;

        [SetUp]
        public override void Setup()
        {
            MockGranter = new Mock<IGrantPermissions>();
            AccountRepository = new Mock<IAccountRepository>();
            MockMediator = new Mock<IMediator>();

            Unit = new Portal.Web.Controllers.ChallengeController(
                MockGranter.Object,
                AccountRepository.Object,
                MockMediator.Object);


            MockContextBase = new Mock<HttpContextBase>();

            MockContextBase.Setup(x => x.Request).Returns(new Mock<HttpRequestBase>().Object);
            MockResponse = new Mock<HttpResponseBase>();
            MockContextBase.Setup(x => x.Response).Returns(MockResponse.Object);
            MockUser = new Mock<IPrincipal>();
            MockContextBase.Setup(x => x.User).Returns(MockUser.Object);

            UnitControllerContext = new ControllerContext(MockContextBase.Object, new RouteData(), Unit);

            Unit.ControllerContext = UnitControllerContext;
        }
    }
}