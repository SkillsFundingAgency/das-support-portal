using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FluentAssertions;
using FluentAssertions.Mvc.Fakes;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Core.Domain.Model;
using SFA.DAS.Support.Portal.Web;
using SFA.DAS.Support.Portal.Web.Services;
using SFA.DAS.Support.Portal.Web.ViewModels;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.SearchController
{
    [TestFixture]
    public sealed class UserControllerTests
    {
        private Portal.Web.Controllers.UserController _sut;
        private Mock<ILog> _mockLogger;
        private Mock<IMappingService> _mockMappingService;
        private Mock<IMediator> _mockMediator;

        [SetUp]
        public void Init()
        {
            _mockLogger = new Mock<ILog>();
            _mockMappingService = new Mock<IMappingService>();
            _mockMediator = new Mock<IMediator>();

            _sut = new Portal.Web.Controllers.UserController(
                _mockMappingService.Object,
                _mockMediator.Object);
        }

        [Test]
        public async Task ShouldSetDefaultSearchUrl()
        {
            var routes = new RouteCollection();

            RouteConfig.RegisterRoutes(routes);

            var context = new Mock<HttpContextBase>();

            var response = new Mock<HttpResponseBase>();

            response.Setup(x => x.ApplyAppPathModifier(It.IsAny<string>())).Returns((string url) => url);

            context.SetupGet(x => x.Request).Returns(new FakeHttpRequest("/", "/"));
            context.SetupGet(x => x.Response).Returns(response.Object);

            _mockMediator.Setup(x => x.SendAsync(It.IsAny<EmployerUserQuery>()))
                .Returns(Task.FromResult(new EmployerUserResponse
                {
                    User = new EmployerUser
                    {
                        FirstName = "Joe",
                        LastName = "Bloggs",
                        Email = "joe@bloggs.com",
                        IsActive = true,
                        IsLocked = false
                    },
                    StatusCode = SearchResponseCodes.Success
                }));

            _sut.Url = new UrlHelper(new RequestContext(context.Object, new RouteData()), routes);

            var result = await _sut.Index("112344", "Bob");
            var vr = result as ViewResult;
            var vm = vr.Model as DetailViewModel;

            AssertionExtensions.Should(result).NotBeNull();
            vm.Should().NotBeNull();

        }
        [Test]
        public async Task ShouldReturnPageNotFoundWhenNoUserRecordFound()
        {
            _mockMediator
                .Setup(x => x.SendAsync(It.IsAny<EmployerUserQuery>()))
                .Returns(Task.FromResult(new EmployerUserResponse()));

            var result = await _sut.Index("112344", "Bob");
            var vr = result as HttpNotFoundResult;

            AssertionExtensions.Should(vr).NotBeNull();
        }
    }
}
