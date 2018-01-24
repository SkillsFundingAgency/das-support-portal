using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.ApplicationServices.Handlers
{
    public class
        AccountDetailTeamMembersHandler : IAsyncRequestHandler<AccountTeamMembersQuery, AccountDetailTeamMembersResponse
        >
    {
        private readonly IAccountRepository _accountRepository;

        public AccountDetailTeamMembersHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AccountDetailTeamMembersResponse> Handle(AccountTeamMembersQuery message)
        {
            var response = new AccountDetailTeamMembersResponse
            {
                StatusCode = SearchResponseCodes.NoSearchResultsFound
            };

            var record = await _accountRepository.Get(message.Id, AccountFieldsSelection.TeamMembers);

            if (record != null)
            {
                response.StatusCode = SearchResponseCodes.Success;
                response.Account = record;
            }

            return response;
        }
    }
}