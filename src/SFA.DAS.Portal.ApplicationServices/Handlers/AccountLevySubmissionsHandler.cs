using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Portal.ApplicationServices.Queries;
using SFA.DAS.Portal.ApplicationServices.Responses;
using SFA.DAS.Portal.Core.Domain.Exceptions;
using SFA.DAS.Portal.Core.Domain.Model;
using SFA.DAS.Portal.Core.Helpers;

namespace SFA.DAS.Portal.ApplicationServices.Handlers
{
    public class AccountLevySubmissionsHandler : IAsyncRequestHandler<AccountLevySubmissionsQuery, AccountLevySubmissionsResponse>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILevySubmissionsRepository _levySubmissionsRepository;
        private readonly IPayeSchemeObfuscator _payeSchemeObfuscator;
        private readonly ILog _logger;

        public AccountLevySubmissionsHandler(IAccountRepository accountRepository, ILevySubmissionsRepository levySubmissionsRepository, IPayeSchemeObfuscator payeSchemeObfuscator, ILog logger)
        {
            _accountRepository = accountRepository;
            _levySubmissionsRepository = levySubmissionsRepository;
            _payeSchemeObfuscator = payeSchemeObfuscator;
            _logger = logger;
        }

        public async Task<AccountLevySubmissionsResponse> Handle(AccountLevySubmissionsQuery message)
        {
            var account = await _accountRepository.Get(message.Id, AccountFieldsSelection.ChallengePayeSchemes);

            if (account == null)
            {
                return new AccountLevySubmissionsResponse
                {
                    StatusCode = AccountLevySubmissionsResponseCodes.AccountNotFound
                };
            }

            var selectedPayeScheme = account.PayeSchemes.ToList()[int.Parse(message.Position)].Ref;
            account.PayeSchemes = ObscurePayeSchemes(account.PayeSchemes);

            try
            {
                var levySubmissions = _levySubmissionsRepository.Get(selectedPayeScheme);

                return new AccountLevySubmissionsResponse
                {
                    StatusCode = AccountLevySubmissionsResponseCodes.Success,
                    LevySubmissions = await levySubmissions,
                    Account = account
                };
            }
            catch (EntityNotFoundException ex)
            {
                _logger.Error(ex, $"Couldn't find declarations for PAYE scheme with ref {ex.Identifier}");

                return new AccountLevySubmissionsResponse
                {
                    Account = account,
                    StatusCode = AccountLevySubmissionsResponseCodes.DeclarationsNotFound
                };
            }
        }

        private IEnumerable<PayeSchemeViewModel> ObscurePayeSchemes(IEnumerable<PayeSchemeViewModel> accountPayeSchemes)
        {
            return accountPayeSchemes.Select(payeSchemeViewModel => new PayeSchemeViewModel
                {
                    AddedDate = payeSchemeViewModel.AddedDate,
                    Ref = _payeSchemeObfuscator.ObscurePayeScheme(payeSchemeViewModel.Ref),
                    Name = payeSchemeViewModel.Name,
                    RemovedDate = payeSchemeViewModel.RemovedDate,
                    DasAccountId = payeSchemeViewModel.DasAccountId
                })
                .ToList();
        }
    }
}