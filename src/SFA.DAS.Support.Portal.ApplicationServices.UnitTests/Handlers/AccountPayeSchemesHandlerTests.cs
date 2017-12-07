using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Handlers;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Handlers
{
    [TestFixture]
    public class AccountPayeSchemesHandlerTests
    {
        private AccountPayeSchemesHandler _unit;
        private Mock<IAccountRepository> _mockAccountRepository;

        [SetUp]
        public void Setup()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _unit = new AccountPayeSchemesHandler(_mockAccountRepository.Object);
        }

        [Test]
        public async Task ItShouldReturnSuccessCodeAndAccountWhenAccountIsFound()
        {
            string id = "123123";
            var accountPayeSchemesQuery = new AccountPayeSchemesQuery(id);
            var account = new Account(){};
            _mockAccountRepository.Setup(r => r.Get(id, AccountFieldsSelection.PayeSchemes))
                .Returns(Task.FromResult(account));

            var response = await _unit.Handle(accountPayeSchemesQuery);
            Assert.AreEqual(SearchResponseCodes.Success, response.StatusCode);
            Assert.AreEqual(account, response.Account);
        }
        [Test]
        public async Task ItShouldReturnNoSearchResultsFoundCodeWhenAccountIsNotFound()
        {
            string id = "123123";
            var accountPayeSchemesQuery = new AccountPayeSchemesQuery(id);
            _mockAccountRepository.Setup(r => r.Get(id, AccountFieldsSelection.PayeSchemes))
                .Returns(Task.FromResult(null  as Account));
            var response = await _unit.Handle(accountPayeSchemesQuery);
            Assert.AreEqual(SearchResponseCodes.NoSearchResultsFound, response.StatusCode);
        }
    }
}