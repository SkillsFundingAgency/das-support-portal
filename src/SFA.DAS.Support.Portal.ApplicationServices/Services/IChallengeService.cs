using System.Collections.Generic;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Support.Portal.ApplicationServices.Services
{
    public interface IChallengeService
    {
        List<int> GetPayeSchemesCharacters(IEnumerable<PayeSchemeViewModel> payeSchemes);
    }
}