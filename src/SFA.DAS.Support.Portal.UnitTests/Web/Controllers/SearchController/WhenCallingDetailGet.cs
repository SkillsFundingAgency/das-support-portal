using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using FluentAssertions;
using FluentAssertions.Mvc.Fakes;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Core.Domain.Model;
using SFA.DAS.Support.Portal.Web;
using SFA.DAS.Support.Portal.Web.ViewModels;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.SearchController
{
    [TestFixture]
    public sealed class WhenCallingDetailGet : WhenTestingSearchController
    {
        [Test]
        public async Task ShouldReturnPageNotFoundWhenNoUserRecordFound()
        {
            MockMediator.Setup(x => x.SendAsync(It.IsAny<EmployerUserQuery>()))
                .Returns(Task.FromResult(new EmployerUserResponse()));

            var result = await Unit.Detail("112344", "Bob");
            var vr = result as HttpNotFoundResult;

            vr.Should().NotBeNull();
        }

        [Test]
        public async Task ShouldSetDefaultSearchUrl()
        {
            var routes = new RouteCollection();

            RouteConfig.RegisterRoutes(routes);

            MockResponseBase.Setup(x => x.ApplyAppPathModifier(It.IsAny<string>())).Returns((string url) => url);

            MockContextBase.SetupGet(x => x.Request).Returns(new FakeHttpRequest("/", "/"));
            MockContextBase.SetupGet(x => x.Response).Returns(MockResponseBase.Object);

            MockMediator.Setup(x => x.SendAsync(It.IsAny<EmployerUserQuery>()))
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

            Unit.Url = new UrlHelper(new RequestContext(MockContextBase.Object, new RouteData()), routes);

            var result = await Unit.Detail("112344", "Bob");
            var vr = result as ViewResult;
            var vm = vr.Model as DetailViewModel;

            result.Should().NotBeNull();

            vm.SearchUrl.ShouldBeEquivalentTo("/search?SearchTerm=Bob");
        }
    }
}