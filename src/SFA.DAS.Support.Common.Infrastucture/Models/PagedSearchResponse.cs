using System.Collections.Generic;

namespace SFA.DAS.Support.Common.Infrastucture.Models
{
    public class PagedSearchResponse<T> where T : class
    {
        public List<T> Results { get; set; }

        public long TotalCount { get; set; }

        public int LastPage { get; set; }
    }
}