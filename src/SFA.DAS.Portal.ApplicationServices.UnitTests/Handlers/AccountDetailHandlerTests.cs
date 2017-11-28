using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Portal.ApplicationServices.Handlers;
using SFA.DAS.Portal.ApplicationServices.Queries;
using SFA.DAS.Portal.ApplicationServices.Responses;
using SFA.DAS.Portal.Core.Domain.Model;

namespace SFA.DAS.Portal.ApplicationServices.UnitTests.Handlers
{
    [TestFixture]
    public sealed class AccountDetailHandlerTests
    {
        [SetUp]
        public void Init()
        {
            _mockService = new Mock<IAccountRepository>();
            _sut = new AccountDetailOrganisationsHandler(_mockService.Object);
        }

        private Mock<IAccountRepository> _mockService;
        private AccountDetailOrganisationsHandler _sut;

        [Test]
        public async Task StatusCodeShouldBeNotFoundIfTheResponseIsNull()
        {
            _mockService.Setup(svc => svc.Get(It.IsAny<string>(), It.IsAny<AccountFieldsSelection>()))
                .Returns(Task.FromResult((Account) null));

            var response = await _sut.Handle(new AccountDetailOrganisationsQuery("FakeAccount"));

            response.StatusCode.Should().Be(SearchResponseCodes.NoSearchResultsFound);
            response.Account.Should().BeNull();
        }

        [Test]
        public async Task StatusCodeShouldBeSuccessIfAnAccountIsReceived()
        {
            _mockService.Setup(svc => svc.Get(It.IsAny<string>(), It.IsAny<AccountFieldsSelection>()))
                .Returns(Task.FromResult(new Account()));

            var response = await _sut.Handle(new AccountDetailOrganisationsQuery("KAKAKAKA"));

            response.StatusCode.Should().Be(SearchResponseCodes.Success);
            response.Account.Should().NotBeNull();
        }
    }
}