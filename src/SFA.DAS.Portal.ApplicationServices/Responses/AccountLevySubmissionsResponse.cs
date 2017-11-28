using HMRC.ESFA.Levy.Api.Types;
using SFA.DAS.Portal.Core.Domain.Model;

namespace SFA.DAS.Portal.ApplicationServices.Responses
{
    public class AccountLevySubmissionsResponse
    {
        public Account Account { get; set; }
        public AccountLevySubmissionsResponseCodes StatusCode { get; set; }
        public LevyDeclarations LevySubmissions { get; set; }
    }
}
