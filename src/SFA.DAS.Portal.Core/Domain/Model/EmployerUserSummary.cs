using System.Collections.Generic;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Portal.Core.Domain.Model
{
    public class EmployerUserSummary : EmployerUser
    {
        public string Href { get; set; }
        public List<AccountDetailViewModel> Accounts { get; set; }
    }
}