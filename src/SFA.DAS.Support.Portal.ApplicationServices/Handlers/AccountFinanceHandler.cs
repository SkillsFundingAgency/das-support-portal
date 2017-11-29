using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.ApplicationServices.Handlers
{
    public class AccountFinanceHandler : IAsyncRequestHandler<AccountFinanceQuery, AccountFinanceResponse>
    {
        private readonly IAccountRepository _accountRepository;

        public AccountFinanceHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AccountFinanceResponse> Handle(AccountFinanceQuery message)
        {
            var response = new AccountFinanceResponse
            {
                StatusCode = SearchResponseCodes.NoSearchResultsFound
            };

            var account = await _accountRepository.Get(message.Id, AccountFieldsSelection.Finance);

            if (account != null)
            {
                response.StatusCode = SearchResponseCodes.Success;
                response.Account = account;
                response.Balance = account.Transactions.Any() ? account.Transactions.First().Balance : await _accountRepository.GetAccountBalance(message.Id);
            }

            return response;
        }
    }
}