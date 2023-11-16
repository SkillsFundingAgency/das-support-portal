using System.Security.Principal;
using System.Web.Mvc;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.Web.Settings;
using SFA.DAS.Support.Portal.Web.ViewModels;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.HomeController
{
    [TestFixture]
    public class HomeControllerTest
    {
        private Mock<IWebConfiguration> _mockWebConfiguration;
        private Portal.Web.Controllers.HomeController _sut;

        [SetUp]
        public void Init()
        {
            _mockWebConfiguration = new Mock<IWebConfiguration>();
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]

        public void StartPage_When_User_Not_Authenticated_ShouldReturnValidViewModel(bool useDfESignIn)
        {
            // arrange
            var controllerContext = new Mock<ControllerContext>();
            var principal = new Moq.Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity.Name).Returns("userName");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            controllerContext.SetupGet(x => x.HttpContext.User.Identity.IsAuthenticated).Returns(false);
            _mockWebConfiguration
                .Setup(x => x.UseDfESignIn)
                .Returns(useDfESignIn);

            _sut = new Portal.Web.Controllers.HomeController(_mockWebConfiguration.Object)
            {
                ControllerContext = controllerContext.Object,
            };

            // sut
            var result = _sut.StartPage();

            var vr = result as ViewResult;

            // assert
            vr.Should().NotBeNull();

            var vm = vr?.Model as StartPageViewModel;
            vm.Should().NotBeNull();
            vm?.UseDfESignIn.Should().Be(useDfESignIn);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]

        public void StartPage_When_User_Authenticated_ShouldReturn_Redirect(bool useDfESignIn)
        {
            // arrange
            var controllerContext = new Mock<ControllerContext>();
            var principal = new Moq.Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity.Name).Returns("userName");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            controllerContext.SetupGet(x => x.HttpContext.User.Identity.IsAuthenticated).Returns(true);
            _mockWebConfiguration
                .Setup(x => x.UseDfESignIn)
                .Returns(useDfESignIn);

            _sut = new Portal.Web.Controllers.HomeController(_mockWebConfiguration.Object)
            {
                ControllerContext = controllerContext.Object,
            };

            // sut
            var result = _sut.StartPage();

            var vr = result as RedirectToRouteResult;

            // assert
            vr.Should().NotBeNull();
            vr?.RouteName.Should().NotBeNull();
        }
    }
}
