using SFA.DAS.Support.Shared;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Support.Portal.ApplicationServices.Responses
{
    //[ExcludeFromCodeCoverage]
    public class SearchResponse
    {
        public int Page { get; set; }

        public int LastPage { get; set; }

        public Dictionary<string, List<string>> Results { get; set; }
        public IEnumerable<SearchResultMetadata> SearchResultsMetadata { get; set; }

        public string SearchTerm { get; set; }

    }
}
