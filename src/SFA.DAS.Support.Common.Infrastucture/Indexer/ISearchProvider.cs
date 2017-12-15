using SFA.DAS.Support.Common.Infrastucture.Models;

namespace SFA.DAS.Support.Common.Infrastucture.Indexer
{
    public interface ISearchProvider<T> where T:class
    {
        PagedSearchResponse<T> Search(string searchText, int pageSize = 10, int pageNumber = 0);
    }
}
