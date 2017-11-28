using System.Collections.Generic;
using SFA.DAS.Portal.Core.Domain.Model;

namespace SFA.DAS.Portal.ApplicationServices.Models
{
    public class EmployerUserSearchResults
    {
        public int Page { get; set; }

        public int LastPage { get; set; }

        public string SearchTerm { get; set; }

        public IEnumerable<EmployerUserSummary> Results { get; set; }
    }
}