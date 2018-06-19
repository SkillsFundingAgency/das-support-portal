using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Support.Common.Infrastucture.Models;
using SFA.DAS.Support.Shared.SearchIndexModel;

namespace SFA.DAS.Support.Portal.ApplicationServices.Responses
{
    [ExcludeFromCodeCoverage]
    public class SearchResponse
    {
        public int Page { get; set; }
        public int LastPage { get; set; }
        public string SearchTerm { get; set; }
        public SearchCategory SearchType { get; set; }
        public PagedSearchResponse<AccountSearchModel> AccountSearchResult { get; set; }
        public PagedSearchResponse<UserSearchModel> UserSearchResult { get; set; }
    }
}