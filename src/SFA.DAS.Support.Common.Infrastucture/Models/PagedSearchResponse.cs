using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Support.Common.Infrastucture.Models
{
    [ExcludeFromCodeCoverage]
    public class PagedSearchResponse<T> where T : class
    {
        public List<T> Results { get; set; }

        public long TotalCount { get; set; }

        public int LastPage { get; set; }
    }
}