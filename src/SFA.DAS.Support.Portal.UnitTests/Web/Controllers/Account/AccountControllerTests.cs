using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Web.Controllers;
using SFA.DAS.Support.Portal.Web.Services;
using SFA.DAS.Support.Portal.Web.ViewModels;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.Account
{
    [TestFixture]
    public sealed class AccountControllerTests
    {
        private AccountController _sut;
        private Mock<ILog> _mockLogger;
        private Mock<IMappingService> _mockMappingService;
        private Mock<IMediator> _mockMediator;
        private Mock<ICheckPermissions> _mockChecker;

        [SetUp]
        public void Init()
        {
            _mockLogger = new Mock<ILog>();
            _mockMappingService = new Mock<IMappingService>();
            _mockMediator = new Mock<IMediator>();
            _mockChecker = new Mock<ICheckPermissions>();

            _sut = new AccountController(
                _mockMediator.Object,
                _mockChecker.Object);
        }

        [Test]
        public async Task ShouldReturnPageNotFoundWhenNoUserRecordFound()
        {
            _mockMediator.Setup(x => x.SendAsync(It.IsAny<AccountDetailOrganisationsQuery>()))
                .Returns(Task.FromResult(new AccountDetailOrganisationsResponse()));

            var result = await _sut.Index("112344", "Bob");
            var vr = result as HttpNotFoundResult;

            AssertionExtensions.Should(vr).NotBeNull();
        }

        [Test]
        public async Task ShouldReturnIncorrectInformationEntered()
        {
            _mockChecker.Setup(x => x.HasPermissions(It.IsAny<HttpRequestBase>(), It.IsAny<HttpResponseBase>(), It.IsAny<IPrincipal>(), It.IsAny<string>()))
                .Returns(true);

            _mockMediator.Setup(x => x.SendAsync(It.IsAny<AccountFinanceQuery>()))
                        .Returns(Task.FromResult(new AccountFinanceResponse() { Account = new Core.Domain.Model.Account(), Balance = 10m, StatusCode = SearchResponseCodes.SearchFailed }));

            var result = await _sut.Finance("vj7gbm", "Bob");
            var vr = result as HttpNotFoundResult;

            AssertionExtensions.Should(vr).NotBeNull();
        }

        [Test]
        public async Task ShouldReturnAddedDate()
        {
            var addedDate = DateTime.Today;

            _mockChecker.Setup(x => x.HasPermissions(It.IsAny<HttpRequestBase>(), It.IsAny<HttpResponseBase>(), It.IsAny<IPrincipal>(), It.IsAny<string>()))
                .Returns(true);

            _mockMediator.Setup(x => x.SendAsync(It.IsAny<AccountLevySubmissionsQuery>()))
                .Returns(Task.FromResult(new AccountLevySubmissionsResponse { Account = new Core.Domain.Model.Account {PayeSchemes = new List<PayeSchemeViewModel> {new PayeSchemeViewModel {AddedDate = addedDate} } } }));

            var result = await _sut.PayeSchemeLevySubmissions("1", "1");
            var vr = result as ViewResult;
            var vm = vr.Model as PayeSchemeLevySubmissionViewModel;

            Assert.AreEqual(addedDate, vm.Account.PayeSchemes.ToList()[0].AddedDate);
        }

        [Test]
        public async Task ShouldReturnName()
        {
            var name = "NAME";

            _mockChecker.Setup(x => x.HasPermissions(It.IsAny<HttpRequestBase>(), It.IsAny<HttpResponseBase>(), It.IsAny<IPrincipal>(), It.IsAny<string>()))
                .Returns(true);

            _mockMediator.Setup(x => x.SendAsync(It.IsAny<AccountLevySubmissionsQuery>()))
                .Returns(Task.FromResult(new AccountLevySubmissionsResponse { Account = new Core.Domain.Model.Account { PayeSchemes = new List<PayeSchemeViewModel> { new PayeSchemeViewModel { Name = name } } } }));

            var result = await _sut.PayeSchemeLevySubmissions("1", "1");
            var vr = result as ViewResult;
            var vm = vr.Model as PayeSchemeLevySubmissionViewModel;

            Assert.AreEqual(name, vm.Account.PayeSchemes.ToList()[0].Name);
        }
    }
}