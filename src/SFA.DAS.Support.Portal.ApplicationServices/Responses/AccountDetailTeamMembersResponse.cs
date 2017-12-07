using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.ApplicationServices.Responses
{
    //[ExcludeFromCodeCoverage]
    public class AccountDetailTeamMembersResponse
    {
        public Account Account { get; set; }
        public SearchResponseCodes StatusCode { get; set; }
    }
}
