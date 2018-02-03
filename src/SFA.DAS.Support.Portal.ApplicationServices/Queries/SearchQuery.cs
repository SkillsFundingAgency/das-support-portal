using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Shared.SearchIndexModel;

namespace SFA.DAS.Support.Portal.ApplicationServices.Queries
{
    public class SearchQuery : IAsyncRequest<SearchResponse>
    {
        public SearchQuery()
        {
            PageSize = 10;
        }

        public string SearchTerm { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public SearchCategory SearchType { get; set; }
    }
}