using System.Collections.Generic;
using HMRC.ESFA.Levy.Api.Types;
using SFA.DAS.Portal.ApplicationServices.Responses;
using SFA.DAS.Portal.Core.Domain.Model;

namespace SFA.DAS.Portal.Web.ViewModels
{
    public class PayeSchemeLevySubmissionViewModel
    {
        public Account Account { get; set; }
        public List<Declaration> LevyDeclarations { get; set; }

        public string PayePosition { get; set; }
        public AccountLevySubmissionsResponseCodes Status { get; set; }
    }
}