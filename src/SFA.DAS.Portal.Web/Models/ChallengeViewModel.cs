using System.Collections.Generic;
using SFA.DAS.Portal.Core.Domain.Model;

namespace SFA.DAS.Portal.Web.Models
{
    public class ChallengeViewModel
    {
        public string Id { get; set; }

        public Account Account { get; set; }

        public string Url { get; set; }

        public List<int> Characters { get; set; }

        public bool HasError { get; set; }
    }
}