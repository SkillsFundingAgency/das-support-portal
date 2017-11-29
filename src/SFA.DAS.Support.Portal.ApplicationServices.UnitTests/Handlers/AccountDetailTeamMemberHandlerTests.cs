using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Handlers;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Handlers
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public sealed class AccountDetailTeamMemberHandlerTests
    {
        [SetUp]
        public void Init()
        {
            _mockService = new Mock<IAccountRepository>();
            _sut = new AccountDetailTeamMembersHandler(_mockService.Object);
        }

        private Mock<IAccountRepository> _mockService;
        private AccountDetailTeamMembersHandler _sut;

        [Test]
        public async Task StatusCodeShouldBeNotFoundIfTheResponseIsNull()
        {
            _mockService.Setup(svc => svc.Get(It.IsAny<string>(), AccountFieldsSelection.TeamMembers))
                .Returns(Task.FromResult((Account) null));

            var response = await _sut.Handle(new AccountTeamMembersQuery("FakeAccount"));

            response.StatusCode.Should().Be(SearchResponseCodes.NoSearchResultsFound);
            response.Account.Should().BeNull();
        }

        [Test]
        public async Task StatusCodeShouldBeSuccessIfAnAccountIsReceived()
        {
            _mockService.Setup(svc => svc.Get(It.IsAny<string>(), AccountFieldsSelection.TeamMembers))
                .Returns(Task.FromResult(new Account()));

            var response = await _sut.Handle(new AccountTeamMembersQuery("ML8D7M"));

            response.StatusCode.Should().Be(SearchResponseCodes.Success);
            response.Account.Should().NotBeNull();
        }
    }
}