using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.ApplicationServices.Handlers
{
    public class AccountPayeSchemesHandler : IAsyncRequestHandler<AccountPayeSchemesQuery, AccountPayeSchemesResponse>
    {
        private readonly IAccountRepository _accountRepository;

        public AccountPayeSchemesHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AccountPayeSchemesResponse> Handle(AccountPayeSchemesQuery message)
        {
            var response = new AccountPayeSchemesResponse
            {
                StatusCode = SearchResponseCodes.NoSearchResultsFound
            };

            var record = await _accountRepository.Get(message.Id, AccountFieldsSelection.PayeSchemes);
            
            if (record != null)
            {
                response.StatusCode = SearchResponseCodes.Success;
                response.Account = record;
            }

            return response;
        }
    }
}