using System.Diagnostics.CodeAnalysis;
using HMRC.ESFA.Levy.Api.Types;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.ApplicationServices.Responses
{
    //[ExcludeFromCodeCoverage]
    public class AccountLevySubmissionsResponse
    {
        public Account Account { get; set; }
        public AccountLevySubmissionsResponseCodes StatusCode { get; set; }
        public LevyDeclarations LevySubmissions { get; set; }
    }
}
