using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Portal.ApplicationServices.Queries;
using SFA.DAS.Portal.ApplicationServices.Responses;
using SFA.DAS.Portal.Core.Domain.Model;

namespace SFA.DAS.Portal.ApplicationServices.Handlers
{
    public class AccountDetailOrganisationsHandler : IAsyncRequestHandler<AccountDetailOrganisationsQuery, AccountDetailOrganisationsResponse>
    {
        private readonly IAccountRepository _accountRepository;

        public AccountDetailOrganisationsHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AccountDetailOrganisationsResponse> Handle(AccountDetailOrganisationsQuery message)
        {
             var response = new AccountDetailOrganisationsResponse
            {
                StatusCode = SearchResponseCodes.NoSearchResultsFound
            };

            var record = await _accountRepository.Get(message.Id.ToUpper(), AccountFieldsSelection.Organisations);
            
            if (record != null)
            {
                response.StatusCode = SearchResponseCodes.Success;
                response.Account = record;
            }

            return response;
        }
    }
}