using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.ApplicationServices.Responses
{
    [ExcludeFromCodeCoverage]
    public class ChallengeResponse
    {
        public Account Account { get; set; }

        public List<int> Characters { get; set; }

        public SearchResponseCodes StatusCode { get; set; }
    }
}