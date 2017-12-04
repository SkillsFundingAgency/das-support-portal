using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.Support.Portal.ApplicationServices.Handlers;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Handlers
{
    [TestFixture]
    public class AccountFinanceHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            Unit = new AccountFinanceHandler(_mockAccountRepository.Object);
        }

        private AccountFinanceHandler Unit;
        private Mock<IAccountRepository> _mockAccountRepository;

        [Test]
        public async Task ItShouldReturnNoSearchResultsWithAnInvalidId()
        {
            var id = "123Invalid";
            _mockAccountRepository.Setup(r => r.Get(
                    It.IsAny<string>(),
                    It.IsAny<AccountFieldsSelection>()))
                .Returns(Task.FromResult(null as Account));

            var accountFinanceQuery = new AccountFinanceQuery(id);

            var response = await Unit.Handle(accountFinanceQuery);

            Assert.AreEqual(SearchResponseCodes.NoSearchResultsFound, response.StatusCode);
        }

        [Test]
        public async Task ItShouldReturnSuccessIfAccountFound()
        {
            var id = "123";
            _mockAccountRepository.Setup(r => r.Get(
                    It.IsAny<string>(),
                    It.Is<AccountFieldsSelection>(x => x == AccountFieldsSelection.Finance)))
                .Returns(Task.FromResult(new Account {Transactions = new List<TransactionViewModel>()}));
            var accountFinanceQuery = new AccountFinanceQuery(id);
            var response = await Unit.Handle(accountFinanceQuery);

            Assert.AreEqual(SearchResponseCodes.Success, response.StatusCode);
        }


        [Test]
        public async Task ItShouldReturnTheBalanceFromTransactions()
        {
            var id = "123";
            _mockAccountRepository.Setup(r => r.Get(
                    It.IsAny<string>(),
                    It.Is<AccountFieldsSelection>(x => x == AccountFieldsSelection.Finance)))
                .Returns(Task.FromResult(new Account
                {
                    Transactions = new List<TransactionViewModel>
                    {
                        new TransactionViewModel {Amount = 1000.00M, Balance = 100.99M}
                    }
                }));
            var accountFinanceQuery = new AccountFinanceQuery(id);
            var response = await Unit.Handle(accountFinanceQuery);

            Assert.AreEqual(100.99M, response.Balance);
        }

        [Test]
        public async Task ItShouldQueryTheBalanceDirectlyIfNoTransactionsFound()
        {
            var id = "123";
            _mockAccountRepository.Setup(r => r.Get(
                    It.IsAny<string>(),
                    It.Is<AccountFieldsSelection>(x => x == AccountFieldsSelection.Finance)))
                .Returns(Task.FromResult(new Account
                {
                    Transactions = new List<TransactionViewModel>
                    {
                       
                    }
                }));
            _mockAccountRepository.Setup(r => r.GetAccountBalance(id)).Returns(Task.FromResult(100.99M));


            var accountFinanceQuery = new AccountFinanceQuery(id);
            var response = await Unit.Handle(accountFinanceQuery);

            _mockAccountRepository.Verify(r=>r.GetAccountBalance(id));

            Assert.AreEqual(100.99M, response.Balance);
        }

    }
}