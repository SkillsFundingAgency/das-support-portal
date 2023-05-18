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

        public void ShouldReturnValidViewModel(bool useDfESignIn)
        {
            // arrange
            _mockWebConfiguration
                .Setup(x => x.UseDfESignIn)
                .Returns(useDfESignIn);

            _sut = new Portal.Web.Controllers.HomeController(_mockWebConfiguration.Object);

            // sut
            var result = _sut.StartPage();

            var vr = result as ViewResult;

            // assert
            vr.Should().NotBeNull();

            var vm = vr.Model as StartPageViewModel;
            vm.Should().NotBeNull();
        }
    }
}
