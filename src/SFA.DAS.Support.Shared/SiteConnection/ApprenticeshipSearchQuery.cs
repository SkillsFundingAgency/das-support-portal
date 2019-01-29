using System.Collections.Generic;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Shared.SiteConnection
{
    public class ApprenticeshipSearchQuery
    {
        public string SearchTerm { get; set; }
        public ApprenticeshipSearchType SearchType { get; set; }

        public IEnumerable<string> ReponseMessages { get; set; }

        public string Id { get; set; }
        public SupportServiceResourceKey Key { get; set; }
        public string ChildId { get; set; }
    }
}