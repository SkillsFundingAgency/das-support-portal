using System.Collections.Generic;
using SFA.DAS.EAS.Account.Api.Types;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Support.Portal.Core.Domain.Model
{
    [ExcludeFromCodeCoverage]
    public class UserSummary
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Href { get; set; }
        public string Email { get; set; }
        public List<AccountDetailViewModel> Accounts { get; set; }
        public UserStatus Status { get; set; }
    }
}
