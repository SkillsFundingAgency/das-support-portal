using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.Web.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class SearchResultsViewModel
    {
        public string SearchTerm { get; set; }
        public IEnumerable<UserSummary> Results { get; set; }
        public int Page { get; set; }
        public int LastPage { get; set; }
        public string ErrorMessage { get; set; }
        public IEnumerable<string> NewResults { get; set; }

        public SearchResultsViewModel()
        {
            Results = new List<UserSummary>();
            Page = 1;
        }
    }
}