using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.ApplicationServices.Models
{
    [ExcludeFromCodeCoverage]
    public class EmployerUserSearchResults
    {
        public int Page { get; set; }

        public int LastPage { get; set; }

        public string SearchTerm { get; set; }

        public IEnumerable<EmployerUserSummary> Results { get; set; }
    }
}