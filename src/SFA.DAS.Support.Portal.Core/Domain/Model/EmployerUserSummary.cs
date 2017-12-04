using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Support.Portal.Core.Domain.Model
{
    [ExcludeFromCodeCoverage]
    public class EmployerUserSummary : EmployerUser
    {
        public string Href { get; set; }
        public List<AccountDetailViewModel> Accounts { get; set; }
    }
}