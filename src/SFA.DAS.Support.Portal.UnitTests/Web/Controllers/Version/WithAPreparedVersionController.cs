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
        protected Mock<IManifestRepository> MockManifestRepository;
        protected Mock<NLog.Logger.ILog> MockLogger;
        protected HttpContextBase HttpContextBase;

        [SetUp]
        public override void Setup()
        {
            MockManifestRepository = new Mock<IManifestRepository>();
            MockLogger = new Mock<NLog.Logger.ILog>();

            Unit = new VersionController(MockManifestRepository.Object, MockLogger.Object);
            HttpContextBase = new FakeContextFactory().Context.Object;
            //HttpContext.Current = HttpContextBase.ApplicationInstance.Context;
        }

        

    }

    public class FakeContextFactory
    {
        public Mock<HttpContextBase> Context;
        public Mock<HttpRequestBase> Request;
        public Mock<HttpResponseBase> Response;
        public Mock<HttpSessionStateBase> Session;
        public Mock<HttpSessionStateBase> Mock;
        public Mock<HttpServerUtilityBase> Server;
        public Mock<IPrincipal> User;
        public Mock<IIdentity> Identity;

        public FakeContextFactory()
        {
            Context = new Mock<HttpContextBase>();
            Request = new Mock<HttpRequestBase>();
            Response = new Mock<HttpResponseBase>();
            Mock = Session = new Mock<HttpSessionStateBase>();
            Server = new Mock<HttpServerUtilityBase>();
            User = new Mock<IPrincipal>();
            Identity = new Mock<IIdentity>();

            WindowsIdentity logonidentity = WindowsIdentity.GetCurrent();

            Request.SetupGet(r => r.LogonUserIdentity).Returns(logonidentity);

            Context.Setup(c => c.Request).Returns(Request.Object);
            Context.Setup(c => c.Response).Returns(Response.Object);
            Context.Setup(c => c.Session).Returns(Session.Object);
            Context.Setup(c => c.Server).Returns(Server.Object);
            Context.Setup(c => c.User).Returns(User.Object);
            User.Setup(c => c.Identity).Returns(Identity.Object);
            Identity.Setup(i => i.IsAuthenticated).Returns(true);
            Identity.Setup(i => i.Name).Returns("admin");
        }
        
    }
}