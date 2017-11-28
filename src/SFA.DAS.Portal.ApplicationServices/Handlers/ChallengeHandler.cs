using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Portal.ApplicationServices.Queries;
using SFA.DAS.Portal.ApplicationServices.Responses;
using SFA.DAS.Portal.ApplicationServices.Services;
using SFA.DAS.Portal.Core.Domain.Model;

namespace SFA.DAS.Portal.ApplicationServices.Handlers
{
    public class ChallengeHandler : IAsyncRequestHandler<ChallengeQuery, ChallengeResponse>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IChallengeService _challengeService;

        public ChallengeHandler(IAccountRepository accountRepository, IChallengeService challengeService)
        {
            _accountRepository = accountRepository;
            _challengeService = challengeService;
        }

        public async Task<ChallengeResponse> Handle(ChallengeQuery message)
        {
            var response = new ChallengeResponse
            {
                StatusCode = SearchResponseCodes.NoSearchResultsFound
            };

            var record = await _accountRepository.Get(message.Id, AccountFieldsSelection.PayeSchemes);
            

            if (record != null)
            {
                response.StatusCode = SearchResponseCodes.Success;
                response.Account = record;
                response.Characters = _challengeService.GetPayeSchemesCharacters(record.PayeSchemes);
            }

            return response;
        }
    }
}