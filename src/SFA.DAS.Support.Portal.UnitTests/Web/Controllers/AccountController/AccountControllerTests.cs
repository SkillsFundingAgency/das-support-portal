using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Core.Domain.Model;
using SFA.DAS.Support.Portal.Web.Services;
using SFA.DAS.Support.Portal.Web.ViewModels;
using AccountDetailViewModel = SFA.DAS.Support.Portal.Web.ViewModels.AccountDetailViewModel;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.AccountController
{
    [TestFixture]
    public sealed class AccountControllerTests
    {
        [SetUp]
        public void Init()
        {
            _mockLogger = new Mock<ILog>();
            _mockMappingService = new Mock<IMappingService>();
            _mockMediator = new Mock<IMediator>();
            _mockChecker = new Mock<ICheckPermissions>();

            _unit = new Portal.Web.Controllers.AccountController(
                _mockMediator.Object,
                _mockChecker.Object);

            var routeData = new RouteData();

            _mockContextBase = new Mock<HttpContextBase>();

            _mockContextBase.Setup(x => x.Request).Returns(new Mock<HttpRequestBase>().Object);
            var requestContext = new RequestContext(_mockContextBase.Object, routeData);
            _mockContextBase.Setup(x => x.Request.RequestContext).Returns(requestContext);
            _mockContextBase.Setup(x => x.Response).Returns(new Mock<HttpResponseBase>().Object);
            _mockContextBase.Setup(x => x.User).Returns(new Mock<IPrincipal>().Object);


            _unitControllerContext = new ControllerContext(_mockContextBase.Object, routeData, _unit);

            _unit.ControllerContext = _unitControllerContext;

            _unit.Url = new UrlHelper(requestContext);
        }

        private Portal.Web.Controllers.AccountController _unit;
        private Mock<ILog> _mockLogger;
        private Mock<IMappingService> _mockMappingService;
        private Mock<IMediator> _mockMediator;
        private Mock<ICheckPermissions> _mockChecker;

        private ControllerContext _unitControllerContext;

        private Mock<HttpContextBase> _mockContextBase;

        [Test]
        public async Task ItShouldReturnNotFoundIfNoTeamMemberMatchFound()
        {
            var id = "123";
            var searchTerm = "something";

            var membersResponse = new AccountDetailTeamMembersResponse
            {
                Account = new Account(),
                StatusCode = SearchResponseCodes.NoSearchResultsFound
            };
            _mockMediator.Setup(x => x.SendAsync(It.IsAny<AccountTeamMembersQuery>())).Returns(
                Task.FromResult(membersResponse));

            var actual = await _unit.TeamMembers(id, searchTerm);
            Assert.IsInstanceOf<HttpNotFoundResult>(actual);
        }

        [Test]
        public async Task ItShouldReturnTeamMembersIfMatchFound()
        {
            var id = "123";
            var searchTerm = "something";

            var membersResponse = new AccountDetailTeamMembersResponse
            {
                Account = new Account
                {
                    TeamMembers = new List<TeamMemberViewModel>
                    {
                        new TeamMemberViewModel {Email = "a@tempuri.org"}
                    }
                },
                StatusCode = SearchResponseCodes.Success
            };
            _mockMediator.Setup(x => x.SendAsync(It.IsAny<AccountTeamMembersQuery>())).Returns(
                Task.FromResult(membersResponse));

            var actual = await _unit.TeamMembers(id, searchTerm);
            Assert.IsInstanceOf<ViewResult>(actual);
            CollectionAssert.IsNotEmpty(((actual as ViewResult).Model as AccountDetailViewModel).Account.TeamMembers);
        }


        [Test]
        public async Task ShouldRedirectToActionOnFinanceWhenNoPermissions()
        {
            _mockChecker.Setup(x => x.HasPermissions(It.IsAny<HttpRequestBase>(), It.IsAny<HttpResponseBase>(),
                    It.IsAny<IPrincipal>(), It.IsAny<string>()))
                .Returns(false);


            var result = await _unit.Finance("1", "1");

            Assert.IsInstanceOf<ActionResult>(result);
        }

        [Test]
        public async Task ShouldRedirectToActionOnPayeSchemeLevySubmissionsWhenNoPermissions()
        {
            _mockChecker.Setup(x => x.HasPermissions(It.IsAny<HttpRequestBase>(), It.IsAny<HttpResponseBase>(),
                    It.IsAny<IPrincipal>(), It.IsAny<string>()))
                .Returns(false);


            var result = await _unit.PayeSchemeLevySubmissions("1", "1");

            Assert.IsInstanceOf<ActionResult>(result);
        }

        [Test]
        public async Task ShouldRedirectToActionOnPayeSchemesWhenNoPermissions()
        {
            _mockMediator.Setup(x => x.SendAsync(It.IsAny<AccountPayeSchemesQuery>()))
                .Returns(Task.FromResult(new AccountPayeSchemesResponse
                {
                    Account = null,
                    StatusCode = SearchResponseCodes.NoSearchResultsFound
                }));

            var result = await _unit.PayeSchemes("1", "1");

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public async Task ShouldReturnIncorrectInformationEntered()
        {
            _mockChecker.Setup(x => x.HasPermissions(It.IsAny<HttpRequestBase>(), It.IsAny<HttpResponseBase>(),
                    It.IsAny<IPrincipal>(), It.IsAny<string>()))
                .Returns(true);

            _mockMediator.Setup(x => x.SendAsync(It.IsAny<AccountFinanceQuery>()))
                .Returns(Task.FromResult(new AccountFinanceResponse
                {
                    Account = new Account(),
                    Balance = 10m,
                    StatusCode = SearchResponseCodes.SearchFailed
                }));

            var result = await _unit.Finance("vj7gbm", "Bob");
            var vr = result as HttpNotFoundResult;

            vr.Should().NotBeNull();
        }

        [Test]
        public async Task ShouldReturnName()
        {
            var name = "NAME";

            _mockChecker.Setup(x => x.HasPermissions(It.IsAny<HttpRequestBase>(), It.IsAny<HttpResponseBase>(),
                    It.IsAny<IPrincipal>(), It.IsAny<string>()))
                .Returns(true);

            _mockMediator.Setup(x => x.SendAsync(It.IsAny<AccountLevySubmissionsQuery>()))
                .Returns(Task.FromResult(new AccountLevySubmissionsResponse
                {
                    Account = new Account
                    {
                        PayeSchemes = new List<PayeSchemeViewModel> {new PayeSchemeViewModel {Name = name}}
                    }
                }));

            var result = await _unit.PayeSchemeLevySubmissions("1", "1");
            var vr = result as ViewResult;
            var vm = vr.Model as PayeSchemeLevySubmissionViewModel;

            Assert.AreEqual(name, vm.Account.PayeSchemes.ToList()[0].Name);
        }

        [Test]
        public async Task ShouldReturnPageNotFoundWhenNoUserRecordFound()
        {
            _mockMediator.Setup(x => x.SendAsync(It.IsAny<AccountDetailOrganisationsQuery>()))
                .Returns(Task.FromResult(new AccountDetailOrganisationsResponse()));

            var result = await _unit.Index("112344", "Bob");
            var vr = result as HttpNotFoundResult;

            vr.Should().NotBeNull();
        }

        [Test]
        public async Task ShouldReturnPayeSchemeLevySubmissionsAddedDate()
        {
            var addedDate = DateTime.Today;

            _mockChecker.Setup(x => x.HasPermissions(It.IsAny<HttpRequestBase>(), It.IsAny<HttpResponseBase>(),
                    It.IsAny<IPrincipal>(), It.IsAny<string>()))
                .Returns(true);

            _mockMediator.Setup(x => x.SendAsync(It.IsAny<AccountLevySubmissionsQuery>()))
                .Returns(Task.FromResult(new AccountLevySubmissionsResponse
                {
                    Account = new Account
                    {
                        PayeSchemes = new List<PayeSchemeViewModel> {new PayeSchemeViewModel {AddedDate = addedDate}}
                    }
                }));

            var result = await _unit.PayeSchemeLevySubmissions("1", "1");
            var vr = result as ViewResult;
            var vm = vr.Model as PayeSchemeLevySubmissionViewModel;

            Assert.AreEqual(addedDate, vm.Account.PayeSchemes.ToList()[0].AddedDate);
        }

        [Test]
        public async Task ShouldReturnPayeSchemesWhenRecordsFound()
        {
            var addedDate = DateTime.Today;
            _mockMediator.Setup(x => x.SendAsync(It.IsAny<AccountPayeSchemesQuery>()))
                .Returns(Task.FromResult(new AccountPayeSchemesResponse
                {
                    Account = new Account
                    {
                        PayeSchemes = new List<PayeSchemeViewModel> {new PayeSchemeViewModel {AddedDate = addedDate}}
                    },
                    StatusCode = SearchResponseCodes.Success
                }));

            var result = await _unit.PayeSchemes("1", "1");

            Assert.IsInstanceOf<ViewResult>(result);
            Assert.IsInstanceOf<AccountDetailViewModel>((result as ViewResult).Model);
            Assert.AreEqual(addedDate,
                ((result as ViewResult).Model as AccountDetailViewModel).Account.PayeSchemes.ToList()[0].AddedDate);
        }

        [Test]
        public async Task ShouldReturnTheFinanceViewWhenHasPermissionsAndRecordFound()
        {
            _mockChecker.Setup(x => x.HasPermissions(It.IsAny<HttpRequestBase>(), It.IsAny<HttpResponseBase>(),
                    It.IsAny<IPrincipal>(), It.IsAny<string>()))
                .Returns(true);

            _mockMediator.Setup(x => x.SendAsync(It.IsAny<AccountFinanceQuery>()))
                .Returns(Task.FromResult(new AccountFinanceResponse
                {
                    Account = new Account(),
                    StatusCode = SearchResponseCodes.Success
                }));


            var result = await _unit.Finance("112344", "Bob");

            Assert.IsInstanceOf<ViewResult>(result);
        }


        [Test]
        public async Task ShouldReturnTheIndexViewWhenRecordFound()
        {
            _mockMediator.Setup(x => x.SendAsync(It.IsAny<AccountDetailOrganisationsQuery>()))
                .Returns(Task.FromResult(new AccountDetailOrganisationsResponse
                {
                    Account = new Account(),
                    StatusCode = SearchResponseCodes.Success
                }));


            var result = await _unit.Index("112344", "Bob");

            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}