using System.Diagnostics.CodeAnalysis;
using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Shared.SearchIndexModel;

namespace SFA.DAS.Support.Portal.ApplicationServices.Queries
{
    public class SearchQuery : IAsyncRequest<SearchResponse>
    {
        public string SearchTerm { get; set; }
        public int Page { get; set; }

        public SearchCategory SearchType { get; set; }
    }
}
