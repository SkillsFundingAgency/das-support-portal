using System.Collections.Generic;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.ApplicationServices.Responses
{
    public class ChallengeResponse
    {
        public Account Account { get; set; }

        public List<int> Characters { get; set; }

        public SearchResponseCodes StatusCode { get; set; }
    }
}
