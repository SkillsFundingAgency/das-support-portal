using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.Web.Models.Error;
using SFA.DAS.Support.Portal.Web.Settings;
using System.Configuration;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.ErrorController
{
    public class WhenCallingTheForbiddenActionMethod
    {
        private Portal.Web.Controllers.ErrorController _controller;
        private Mock<IWebConfiguration> _mockWebConfiguration;
        private Mock<HttpResponseBase> _mockHttpResponseBase;
        private Mock<HttpContextBase> _mockHttpContext;

        [SetUp]
        public void Setup()
        {
            _mockWebConfiguration = new Mock<IWebConfiguration>();
            _mockHttpContext = new Mock<HttpContextBase>();
            _mockHttpResponseBase = new Mock<HttpResponseBase>();
        }

        //[TestCase("test", "https://test-services.signin.education.gov.uk/approvals/select-organisation?action=request-service", true)]
        //[TestCase("pp", "https://test-services.signin.education.gov.uk/approvals/select-organisation?action=request-service", true)]
        //[TestCase("local", "https://test-services.signin.education.gov.uk/approvals/select-organisation?action=request-service", false)]
        //[TestCase("prd", "https://services.signin.education.gov.uk/approvals/select-organisation?action=request-service", false)]
        //public void When_Forbidden_Then_ViewIsReturned(string env, string helpLink, bool useDfESignIn)
        //{
        //    //arrange
        //    ConfigurationManager.AppSettings["ResourceEnvironmentName"] = env;
        //    _mockWebConfiguration.Setup(x => x.UseDfESignIn).Returns(useDfESignIn);
        //    _controller = new Portal.Web.Controllers.ErrorController(_mockWebConfiguration.Object);

        //    _mockHttpResponseBase.Setup(x => x.StatusCode).Returns(403);
        //    _mockHttpContext.SetupGet(c => c.Response).Returns(_mockHttpResponseBase.Object);


        //    _controller.ControllerContext = new ControllerContext(
        //        _mockHttpContext.Object,
        //        new RouteData(),
        //        _controller);

        //    //sut
        //    var result = _controller.Forbidden();

        //    //assert
        //    Assert.That(result, Is.Not.Null);

        //    var actualModel = result.Model as Error403ViewModel;
        //    Assert.That(actualModel?.HelpPageLink, Is.EqualTo(helpLink));
        //    Assert.That(actualModel?.UseDfESignIn, Is.EqualTo(useDfESignIn));
        //    Assert.That(_controller.Response.StatusCode, Is.EqualTo((int)HttpStatusCode.Forbidden));
        //}
    }
}
