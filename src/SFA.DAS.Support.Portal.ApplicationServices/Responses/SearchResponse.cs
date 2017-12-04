using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Support.Portal.ApplicationServices.Responses
{
    [ExcludeFromCodeCoverage]
    public class SearchResponse
    {
        public IEnumerable<string> Results { get; set; }
    }
}
