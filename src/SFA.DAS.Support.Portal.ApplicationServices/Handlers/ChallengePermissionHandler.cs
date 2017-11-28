using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.ApplicationServices.Handlers
{
    public class ChallengePermissionHandler : IAsyncRequestHandler<ChallengePermissionQuery, ChallengePermissionResponse>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IChallengeRepository _challengeRepository;

        public ChallengePermissionHandler(IAccountRepository accountRepository, IChallengeRepository challengeRepository)
        {
            _accountRepository = accountRepository;
            _challengeRepository = challengeRepository;
        }

        public async Task<ChallengePermissionResponse> Handle(ChallengePermissionQuery message)
        {
            var response = new ChallengePermissionResponse
            {
                Id = message.Id,
                Url = message.Url,
                IsValid = false
            };

            int balance;

            if (string.IsNullOrEmpty(message.Balance)
                || string.IsNullOrEmpty(message.ChallengeElement1)
                || string.IsNullOrEmpty(message.ChallengeElement2)
                || !int.TryParse(message.Balance.Split('.')[0].Replace("£", string.Empty), out balance)
                || message.ChallengeElement1.Length != 1
                || message.ChallengeElement2.Length != 1)
            {
                return response;
            }

            var record = await _accountRepository.Get(message.Id, AccountFieldsSelection.ChallengePayeSchemes);

            if (record != null)
            {
                var isValid = await _challengeRepository.CheckData(record, message);

                response.IsValid = isValid;
            }

            return response;
        }
    }
}