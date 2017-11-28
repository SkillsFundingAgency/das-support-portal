using SFA.DAS.Portal.Core.Domain.Model;

namespace SFA.DAS.Portal.ApplicationServices.Responses
{
    public class AccountDetailTeamMembersResponse
    {
        public Account Account { get; set; }
        public SearchResponseCodes StatusCode { get; set; }
    }
}
