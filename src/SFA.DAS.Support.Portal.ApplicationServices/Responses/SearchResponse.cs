using SFA.DAS.Support.Shared;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Support.Portal.ApplicationServices.Responses
{
    //[ExcludeFromCodeCoverage]
    public class SearchResponse
    {
        public Dictionary<string, List<string>> Results { get; set; }
        public IEnumerable<SearchResultMetadata> SearchResultsMetadata { get; set; }

    }
}
