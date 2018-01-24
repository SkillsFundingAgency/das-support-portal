using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Support.Shared.SearchIndexModel;

namespace SFA.DAS.Support.Portal.Web.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class SearchResultsViewModel
    {
        public SearchResultsViewModel()
        {
            Page = 1;
        }

        public string SearchTerm { get; set; }
        public SearchCategory SearchType { get; set; }
        public int Page { get; set; }
        public int LastPage { get; set; }
        public string ErrorMessage { get; set; }

        public IEnumerable<AccountSearchModel> AccountSearchResults { get; set; }
        public int TotalAccountSearchItems { get; set; }

        public IEnumerable<UserSearchModel> UserSearchResults { get; set; }
        public int TotalUserSearchItems { get; set; }
    }
}